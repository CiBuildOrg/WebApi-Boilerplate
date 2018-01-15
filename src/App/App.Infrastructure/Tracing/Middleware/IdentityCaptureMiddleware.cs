using Microsoft.Owin;
using System.Threading.Tasks;

namespace App.Infrastructure.Tracing.Middleware
{
    /// <summary>
    /// Identity capture middleware for logs
    /// </summary>
    public class IdentityCaptureMiddleware : OwinMiddleware
    {
        private const string AnonymousCaller = "(anonymous)";
        private readonly IdentityCaptureOptions _options;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        public IdentityCaptureMiddleware(OwinMiddleware next, IdentityCaptureOptions options) : base(next)
        {
            _options = options;
        }

        /// <summary>
        /// Invoking app func
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task Invoke(IOwinContext context)
        {
            if (!_options.Trace)
            {
                return Next.Invoke(context);
            }

            string identity = string.Empty;
            if (!_options.CaptureCallerIdentity)
            {
                identity = AnonymousCaller;
            }
            else
            {
                var request = context.Request;
                identity = request.User != null && request.User.Identity.IsAuthenticated ?
                        request.User.Identity.Name :
                        "(anonymous)";
            }

            _options.HttpTrackingEntry.CallerIdentity = identity;
            return Next.Invoke(context);
        }
    }
}
