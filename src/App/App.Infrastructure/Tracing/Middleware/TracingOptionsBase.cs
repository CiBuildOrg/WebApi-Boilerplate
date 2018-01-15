namespace App.Infrastructure.Tracing.Middleware
{
    public abstract class TracingOptionsBase
    {
        /// <summary>
        /// Should trace
        /// </summary>
        public bool Trace { get; set; }
    }
}
