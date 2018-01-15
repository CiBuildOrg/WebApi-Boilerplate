using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using App.Infrastructure.Security;
using System.Data.Entity;
using Microsoft.Owin;

namespace App.Api.Security
{
    /// <summary>
    /// Claims transform middleware
    /// </summary>
    public class ClaimsTransformationMiddleware : OwinMiddleware
    {
        private const string AppFuncValidationMessage = "AppFunc of next component";
        private readonly OwinMiddleware _nextComponent;
        private readonly ClaimsTransformationMiddlewareOptions _options;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="appFunc"></param>
        /// <param name="options"></param>
        public ClaimsTransformationMiddleware(OwinMiddleware appFunc, ClaimsTransformationMiddlewareOptions options) : base(appFunc)
        {
            _nextComponent = appFunc ?? throw new ArgumentNullException(AppFuncValidationMessage);
            _options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task Invoke(IOwinContext context)
        {

            ClaimsPrincipal claimsPrincipal = _options.GetClaim(context);

            if (claimsPrincipal != null)
            {
                if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
                {
                    var nameIdentifierClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == _options.NameIdentifier);
                    var userIdValue = nameIdentifierClaim?.Value;
                    if (userIdValue != null)
                    {
                        var userId = Guid.Parse(userIdValue);
                        var userProfile = _options.Context.Users.Include(x => x.ProfileInfo).SingleOrDefault(x => x.Id == userId);
                        if (userProfile != null)
                        {
                            Thread.CurrentPrincipal = new AppClaimsPrincipal(new ClaimsPrincipal(claimsIdentity), userProfile.ProfileInfo);
                        }
                    }
                }
            }

            return Next.Invoke(context);
        }
    }
}