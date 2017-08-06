using System;
using System.Threading.Tasks;
using App.Core.Extensions;
using App.Entities.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace App.Api.Security
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
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
            var manager = context.OwinContext.Get<RefreshTokenManager>();
            var lifeTime = context.OwinContext.Get<string>(Startup.ClientRefreshTokenLifeTimePropertyName);

            var token = new RefreshToken
            {
                Id = refreshTokenId.GetHash(),
                ClientId = clientId,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddSeconds(Convert.ToDouble(lifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await manager.AddRefreshToken(token);
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

            var refreshToken = await context.OwinContext.Get<RefreshTokenManager>().FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = await context.OwinContext.Get<RefreshTokenManager>().RemoveRefreshToken(hashedTokenId);
            }
        }
    }
}