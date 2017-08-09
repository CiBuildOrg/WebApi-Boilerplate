using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using App.Entities.Security;

namespace App.Security.Infrastructure
{
    /// <summary>
    /// This represents authorization by claims
    /// </summary>
    public class ClaimsAuthorizationAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Claim type
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// Claim Value
        /// </summary>
        public string ClaimValue { get; set; }

        /// <summary>
        /// Authorization check
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as AppClaimsPrincipal;

            if (principal != null && !principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

                return Task.FromResult<object>(null);
            }

            if (string.IsNullOrWhiteSpace(ClaimType) || string.IsNullOrWhiteSpace(ClaimValue))
                return Task.FromResult<object>(null);
            if (principal == null || principal.HasClaim(c => c.Type == ClaimType && c.Value == ClaimValue))
                return Task.FromResult<object>(null);

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            return Task.FromResult<object>(null);
        }
    }
}