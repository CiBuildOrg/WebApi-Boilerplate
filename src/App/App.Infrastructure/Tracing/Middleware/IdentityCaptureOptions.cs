namespace App.Infrastructure.Tracing.Middleware
{
    public class IdentityCaptureOptions : TracingOptionsBase
    {
        /// <summary>
        /// Request/response tracking entity
        /// </summary>
        public HttpEntry HttpTrackingEntry { get; set; }

        /// <summary>
        /// Should capture caller identity for logs?
        /// </summary>
        public bool CaptureCallerIdentity { get; set; }
    }
}
