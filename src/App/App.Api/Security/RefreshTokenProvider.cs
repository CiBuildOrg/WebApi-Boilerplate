using System;
using System.Threading.Tasks;
using App.Core.Extensions;
using App.Entities.Security;
using App.Security.Contracts;
using Microsoft.Owin.Security.Infrastructure;

namespace App.Api.Security
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly IRefreshTokenManager _refreshTokenManager;
        private readonly IApplicationUserManager _applicationUserManager;

        public RefreshTokenProvider(IRefreshTokenManager refreshTokenManager, IApplicationUserManager applicationUserManager)
        {
            _refreshTokenManager = refreshTokenManager;
            _applicationUserManager = applicationUserManager;
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientId = context.Ticket.Properties.Dictionary[Startup.ClientPropertyName];

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("N");
            var lifeTime = context.OwinContext.Get<string>(Startup.ClientRefreshTokenLifeTimePropertyName);
            var userId = context.OwinContext.Get<string>(Startup.UserPropertyName);
            var user = _applicationUserManager.FindApplciationUser(Guid.Parse(userId));

            if (user == null)
            {
                return;
            }

            var token = new RefreshToken
            {
                Id = refreshTokenId.GetHash(),
                ClientId = Guid.Parse(clientId),
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddSeconds(Convert.ToDouble(lifeTime)),
                User = user
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await _refreshTokenManager.AddRefreshToken(token);
            if (result)
                context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(Startup.ClientAllowedOriginPropertyName);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var hashedTokenId = context.Token.GetHash();
            var refreshToken = await _refreshTokenManager.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                context.OwinContext.Set(Startup.UserPropertyName, refreshToken.User.Id.ToString());
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                await _refreshTokenManager.RemoveRefreshToken(hashedTokenId);
            }
        }
    }
}