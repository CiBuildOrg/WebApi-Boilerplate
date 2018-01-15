using Microsoft.Owin;
using System;

namespace App.Infrastructure.Tracing.Middleware
{
    /// <summary>
    /// A simple class to hold details of an HTTP call.
    /// </summary>
    public sealed class HttpEntry
    {
        public static bool IsText(string contentTypeHeader)
        {
            var contentType = new System.Net.Mime.ContentType(contentTypeHeader);
            var mediaType = contentType.MediaType;

            return (
                mediaType.StartsWith("text/") ||
                mediaType.EndsWith("/json") ||
                mediaType.EndsWith("/xml")
            );
        }

        /// <summary>
        /// Identity of the caller.
        /// </summary>
        public string CallerIdentity { get; set; }

        /// <summary>
        /// Timestamp at which the HTTP call took place.
        /// </summary>
        public DateTime CallDateTime { get; set; }

        /// <summary>
        /// Verb associated with the HTTP call.
        /// </summary>
        public string Verb { get; set; }

        /// <summary>
        /// Http request URI.
        /// </summary>
        public Uri RequestUri { get; set; }

        /// <summary>
        /// Http request body, if any.
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// Http response status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Http response status line.
        /// </summary>
        public string ReasonPhrase { get; set; }

        public IHeaderDictionary RequestHeaders { get; set; }

        public IHeaderDictionary ResponseHeaders { get; set; }
        /// <summary>
        /// Http response body.
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Timestamp humanized representing the duration of the HTTP call.
        /// </summary>
        public string CallDuration { get; set; }
    }
}
