using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Entities.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace App.Api.Security
{
    public class OauthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IRefreshTokenManager _refreshTokenManager;
        private readonly UserManager<ApplicationUser, Guid> _applicationUserManager;

        public OauthProvider(IRefreshTokenManager refreshTokenManager, UserManager<ApplicationUser, Guid> applicationUserManager)
        {
            _refreshTokenManager = refreshTokenManager;
            _applicationUserManager = applicationUserManager;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            if (!context.TryGetBasicCredentials(out string clientId, out string clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (string.IsNullOrWhiteSpace(context.ClientId))
            {
                context.Rejected();
                context.SetError("invalid_clientId", "ClientId should be sent.");

                return Task.FromResult<object>(null);
            }

            var client = _refreshTokenManager.FindClient(Guid.Parse(context.ClientId));

            if (client == null)
            {
                context.Rejected();
                context.SetError("invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");

                return Task.FromResult<object>(null);
            }

            // Javascript client 
            if (client.ApplicationType == ApplicationType.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.Rejected();
                    context.SetError("invalid_clientId", "Client secret should be sent.");

                    return Task.FromResult<object>(null);
                }

                if (clientSecret != client.Secret)
                {
                    context.Rejected();
                    context.SetError("invalid_clientId", "Client secret is invalid.");

                    return Task.FromResult<object>(null);
                }
            }

            if (!client.Active)
            {
                context.Rejected();
                context.SetError("invalid_clientId", "Client is inactive.");

                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set(Startup.ClientAllowedOriginPropertyName, client.AllowedOrigin);
            context.OwinContext.Set(Startup.ClientRefreshTokenLifeTimePropertyName, client.RefreshTokenLifeTime.ToString());

            context.Validated();

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(Startup.ClientAllowedOriginPropertyName);

            if (string.IsNullOrWhiteSpace(allowedOrigin))
            {
                allowedOrigin = "*"; 
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var user = await _applicationUserManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.Rejected();
                context.SetError("invalid_grant", "The username or password is incorrect.");

                return;
            }

            if (!user.EmailConfirmed)
            {
                context.Rejected();
                context.SetError("invalid_grant", "User did not confirm email.");
                return;
            }

            var oAuthIdentity = await user.GenerateUserIdentityAsync(_applicationUserManager, "JWT");

            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            oAuthIdentity.AddClaim(new Claim("user_id", user.Id.ToString()));
            oAuthIdentity.AddClaim(new Claim("sub", context.UserName));

            context.OwinContext.Set(Startup.UserPropertyName, user.Id.ToString());
            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                { Startup.ClientPropertyName, context.ClientId ?? string.Empty  },
                //{ "userName", context.UserName }
            });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);

            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // grant_type = refresh_token
            var originalClient = context.Ticket.Properties.Dictionary[Startup.ClientPropertyName];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.Rejected();
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");

                return Task.FromResult<object>(null);
            }

            // change auth ticket for refresh token request
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}