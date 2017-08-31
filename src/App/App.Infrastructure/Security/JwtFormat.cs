using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using App.Core;
using App.Core.Contracts;
using App.Entities.Security;
using App.Infrastructure.Contracts;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Thinktecture.IdentityModel.Tokens;

namespace App.Infrastructure.Security
{
    public class JwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly IRefreshTokenManager _refreshTokenManager;
        
        private readonly string _issuer;
        private List<Client> _allowedAudiences = new List<Client>();

        public JwtFormat(IConfiguration configuration, IRefreshTokenManager refreshTokenManager)
        {
            _refreshTokenManager = refreshTokenManager;
            
            _issuer = configuration.GetString(SecurityKeys.Issuer); // issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            string audienceId = data.Properties.Dictionary[OwinEnvironment.ClientPropertyName];
            var client = _refreshTokenManager.FindClient(Guid.Parse(audienceId));
            string symmetricKeyBase64 = client.Secret;

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyBase64);
            var signingCredentials = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;

            if(issued == null)
                throw new Exception("Issued is null");

            var expires = data.Properties.ExpiresUtc;

            if (expires == null)
                throw new Exception("Expires is null");

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, 
                issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);

            return jwt;
        }   

        private bool ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParam)
        {
            var enumerable = audiences as string[] ?? audiences.ToArray();
            if (!_allowedAudiences.Select(c => c.Id.ToString()).Intersect(enumerable).Any())
            {
                var validated = AllowedAudience().Select(c => c.Id.ToString().ToLower()).Intersect(enumerable.Select(x => x.ToLower())).Any();
                return validated;
            }

            _allowedAudiences.Clear();
            _allowedAudiences = AllowedAudience().ToList();

            return AllowedAudience().Select(c => c.Id.ToString().ToLower()).Intersect(enumerable.Select( x => x.ToLower())).Any();
        }

        private IEnumerable<Client> AllowedAudience() => _refreshTokenManager.GetAllowedClients();


        public AuthenticationTicket Unprotect(string protectedText)
        {
            var handler = new JwtSecurityTokenHandler();
            var issuerSigningTokens = new SecurityTokensTokens(_issuer) {Audiences = AllowedAudience};
            var validationParams = new TokenValidationParameters
            {
                AudienceValidator =  ValidateAudience,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningTokens = issuerSigningTokens,
                ClockSkew = TimeSpan.Zero // default value of this property is 5, it adds 5 mins to expiration time.
            };

            var result = handler.ValidateToken(protectedText, validationParams, out SecurityToken _);
            var claimsIdentity = new ClaimsIdentity(result.Claims, "JWT");

          
            var ticket = new AuthenticationTicket(claimsIdentity, null);
            return ticket;
        }
    }
}