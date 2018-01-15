using App.Core.Contracts;
using App.Database;
using System.Collections.Generic;

namespace App.Infrastructure.Tracing.Middleware
{
    /// <summary>
    /// Options for configuring the <see cref="HttpTrackingMiddleware"/> class.
    /// </summary>
    public sealed class BookKeeperOptions : TracingOptionsBase
    {
        /// <summary>
        /// Db context
        /// </summary>
        public LogsDatabaseContext Context { get; set; }
        /// <summary>
        /// The http tracking entry
        /// </summary>
        public HttpEntry HttpTrackingEntry { get; set; }
        /// <summary>
        /// The maximum number of bytes from the request to persist to durable storage.
        /// </summary>
        public int MaximumRecordedRequestLength { get; set; }

        /// <summary>
        /// The maximum number of bytes from the response to persist to durable storage.
        /// </summary>
        public int MaximumRecordedResponseLength { get; set; }

        /// <summary>
        /// What headers not to capture
        /// </summary>
        public List<string> RequestHeaderCaptureExceptions { get; set; }

        /// <summary>
        /// Request/response anonymizer
        /// </summary>
        public ILogBookAnonymizer Anonymizer { get; set; }

        /// <summary>
        /// Prefix to relative path that will be logged
        /// </summary>
        public List<string> UrlPrefixes { get; set; }
    }
}
