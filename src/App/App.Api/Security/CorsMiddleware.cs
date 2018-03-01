using System.Threading.Tasks;
using Microsoft.Owin;

namespace App.Api.Security
{
    /// <summary>
    /// Cors middleware
    /// </summary>
    public class CorsMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next"></param>
        public CorsMiddleware(OwinMiddleware next) : base(next)
        {
        }

        /// <summary>
        /// Invoke function
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Method == "OPTIONS")
            {
                //poor mans cors.
                if(!context.Response.Headers.ContainsKey("Access-Control-Allow-Methods"))
                {
                    context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, OPTIONS, PUT, DELETE" });
                }

                if(!context.Response.Headers.ContainsKey("Access-Control-Allow-Headers"))
                {
                    context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "accept", "authorization", "content-type", "version", "apporigin", "x-ms-request-id", "x-ms-request-root-id" });
                }

                if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                }

                context.Response.StatusCode = 200;
                await Task.FromResult(0);
                return;
            }

            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Methods"))
            {
                context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, OPTIONS, PUT, DELETE" });
            }

            if(!context.Response.Headers.ContainsKey("Access-Control-Allow-Headers"))
            {
                context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "accept", "authorization", "content-type", "version", "apporigin", "x-ms-request-id", "x-ms-request-root-id" });
            }

            if(!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            }

            await Next.Invoke(context);
        }
    }
}