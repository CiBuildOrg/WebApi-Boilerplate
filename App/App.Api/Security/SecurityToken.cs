using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens;
using System.Linq;
using App.Entities;
using App.Entities.Security;
using App.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace App.Api.Security
{
    public class SecurityTokensTokens : AbstractAudience<SecurityToken>
    {
        private readonly string _issuer;

        public SecurityTokensTokens(string issuer)
        {
            _issuer = issuer;
        }

        public override IEnumerator<SecurityToken> GetEnumerator()
        {
            return Audiences()
                  .SelectMany(audience => 
                                      new SymmetricKeyIssuerSecurityTokenProvider(
                                          _issuer, 
                                          TextEncodings.Base64Url.Decode(audience.Secret)).SecurityTokens)
                  .GetEnumerator();
        }
    }
}