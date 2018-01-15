using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http.ExceptionHandling;
using App.Core.Utils;

namespace App.Api.ErrorHandling
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    partial class OopsExceptionHandler : ExceptionHandler
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
    }
}