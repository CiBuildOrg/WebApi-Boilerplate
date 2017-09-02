using System.Net;
using App.Exceptions;

namespace App.Api.ErrorHandling
{
    public static class ExceptionMapper
    {
        public static HttpStatusCode MapException(System.Exception context)
        {
            if (context.GetType().IsSubclassOf(typeof(AbstractRequestException)))
            {
                //var exception = (AbstractRequestException)context;
                return HttpStatusCode.InternalServerError;
            }
            if (context is NotAuthenticatedException)
            {
                return HttpStatusCode.Unauthorized;
            }
            if (context is NotAllowedException)
            {
                return HttpStatusCode.MethodNotAllowed;
            }
            return HttpStatusCode.InternalServerError;
        }
    }
}