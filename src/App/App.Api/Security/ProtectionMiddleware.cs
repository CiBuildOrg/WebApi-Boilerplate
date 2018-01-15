using Microsoft.Owin;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Api.Security
{
    /// <summary>
    /// protect middleware 
    /// </summary>
    public class ProtectionMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware _nextComponent;
        private readonly ProtectionMiddlewareOptions _options;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="appFunc">Next middleware in the OWIN pipeline</param>
        /// <param name="options"><see cref="ProtectionMiddlewareOptions">Middleware options</see></param>
        public ProtectionMiddleware(OwinMiddleware appFunc, ProtectionMiddlewareOptions options) : base(appFunc)
        {
            _nextComponent = appFunc ?? throw new ArgumentNullException("AppFunc of next component");
            _options = options;
        }

        /// <summary>
        /// Invoke function
        /// 
        /// Checks if the path is right and if the current user is authenticated and has permissions to view that path
        ///
        /// </summary>
        /// <param name="context">Owin context</param>
        /// <returns></returns>
        public override Task Invoke(IOwinContext context)
        {
            if (!context.Request.Path.HasValue)
            {
                return Next.Invoke(context);
            }

            var config = _options.Configs.SingleOrDefault(x => context.Request.Path.Value.Contains(x.ProtectPath));
            if (config == null)
            {
                return Next.Invoke(context);
            }

            ClaimsPrincipal claimsPrincipal = context.Authentication.User;
            if (claimsPrincipal == null)
            {
                return Next.Invoke(context);
            }

            var canSee = config.AllowedRoles.Any(claimsPrincipal.IsInRole);
            if (!canSee)
            {
                context.Response.Redirect(config.RedirectUrl);
                return Task.FromResult(0);
            }
            else
            {
                return Next.Invoke(context);
            }
        }
    }
}