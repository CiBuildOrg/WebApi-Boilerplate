using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using App.Exceptions;

namespace App.Api.ErrorHandling
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception.GetType().IsSubclassOf(typeof(AbstractRequestException)))
            {
                var exception = (AbstractRequestException)context.Exception;

                var exceptionStatusCode = ExceptionMapper.MapException(exception);
                context.Response = context.Request.CreateResponse(exceptionStatusCode, ExceptionToDto(exception));
            }
            else
            {
                if (context.Exception is NotAuthenticatedException)
                {
                    context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                else if (context.Exception is NotAllowedException)
                {
                    context.Response = context.Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
                }
                else
                {
                    context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
        }

        private static ExceptionResponse ExceptionToDto(AbstractRequestException exception)
        {
            if (exception == null)
            {
                return null;
            }

            return new ExceptionResponse
            {
                Code = exception.Code,
                Message = exception.ExceptionMessage,
                Dump = exception.DataDump
            };
        }
    }
}