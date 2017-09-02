using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace App.Api.ErrorHandling
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ExceptionHandler : IExceptionHandler
    {
        public virtual Task HandleAsync(ExceptionHandlerContext context,
            CancellationToken cancellationToken)
        {
            return !ShouldHandle(context) ? Task.FromResult(0) : HandleAsyncCore(context, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task HandleAsyncCore(ExceptionHandlerContext context,
            CancellationToken cancellationToken)
        {
            HandleCore(context);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected virtual void HandleCore(ExceptionHandlerContext context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
}