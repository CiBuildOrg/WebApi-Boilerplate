using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using App.Core.Utils;

namespace App.Api.ErrorHandling
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    class OopsExceptionHandler : ExceptionHandler
    {
        protected override void HandleCore(ExceptionHandlerContext context)
        {
            Exception exception = null;

            //if(context.ExceptionContext.Exception.GetType().IsSubclassOf(typeof(AbstractRequestException)))
            //    return;

            if (context.ExceptionContext.Exception != null)
            {
                exception = context.ExceptionContext.Exception;
            }
            else if (context.Exception != null)
            {
                exception = context.Exception;
            }

            if (exception != null)
            {
                EventLogger.WriteError(exception, "Happening in the global unhandled exception handler");
            }

            context.Result = new TextPlainErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = "Oops! Something went wrong.",
            };
        }

        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response =
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent(Content),
                        RequestMessage = Request
                    };

                return Task.FromResult(response);
            }
        }
    }
}