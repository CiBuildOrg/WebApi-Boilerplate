using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using App.Database;
using App.Infrastructure.Security;
using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace App.Api.Security
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class CustomAuthenticationMessageHandler : DelegatingHandler
    {
        private readonly DatabaseContext _context;

        public CustomAuthenticationMessageHandler(DatabaseContext context)
        {
            _context = context;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var incomingPrincipal = request.GetRequestContext().Principal as ClaimsPrincipal;
            if (incomingPrincipal == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var nameIdentifierClaim = incomingPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var userIdValue = nameIdentifierClaim?.Value;
            if (userIdValue == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var userId = Guid.Parse(userIdValue);
            var userProfile = _context.Users.Include(x => x.ProfileInfo).SingleOrDefault(x => x.Id == userId);

            if (userProfile != null)
            {
                request.GetRequestContext().Principal =
                    new AppClaimsPrincipal(incomingPrincipal, userProfile.ProfileInfo);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}