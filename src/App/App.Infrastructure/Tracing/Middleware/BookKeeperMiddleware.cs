using App.Api.Enum;
using App.Database;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace App.Infrastructure.Tracing.Middleware
{
    /// <summary>
    /// A simple Owin Middleware to capture HTTP requests and responses
    /// and store details of the call into a durable store.
    /// </summary>
    public sealed class BookKeeperMiddleware : OwinMiddleware
    {
        private const string ContentTypeHeader = "Content-Type";

        private readonly BookKeeperOptions _options;

        private readonly int _maxRecordedRequestLength = int.MaxValue;
        private readonly int _maxRecordedResponseLength = int.MaxValue;

        /// <summary>
        /// Initialize a new instance of the <see cref="HttpTrackingMiddleware"/> class.
        /// </summary>
        /// <param name="next">A reference to the next OwinMiddleware object in the chain.</param>
        /// <param name="options">A reference to an <see cref="HttpTrackingOptions"/> class.</param>
        public BookKeeperMiddleware(OwinMiddleware next, BookKeeperOptions options)
            : base(next)
        {
            _options = options;
            _maxRecordedRequestLength = options.MaximumRecordedRequestLength;
            _maxRecordedResponseLength = options.MaximumRecordedResponseLength;
        }

        /// <summary>
        /// Processes the incoming HTTP call and capture details about
        /// the request, the response, the identity of the caller and the
        /// call duration to persistent storage.
        /// </summary>
        /// <param name="context">A reference to the Owin context.</param>
        /// <returns />
        public override async Task Invoke(IOwinContext context)
        {
            // if tracing is not enabled, don't do anything
            if (!_options.Trace || !_options.UrlPrefixes.Any(context.Request.Uri.AbsolutePath.StartsWith))
            {
                await Next.Invoke(context);
                return;
            }

            // start stopwatch to record call duration 
            var sw = new Stopwatch();
            sw.Start();

            // record call time
            _options.HttpTrackingEntry.CallDateTime = DateTime.UtcNow;

            var request = context.Request;
            var response = context.Response;
            // replace the request stream in order to intercept downstream reads

            // Buffering mvc reponse
            HttpResponse httpResponse = HttpContext.Current.Response;
            StreamHelper outputCapture = new StreamHelper(httpResponse.Filter);
            httpResponse.Filter = outputCapture;

            // Buffering Owin response if any 
            IOwinResponse owinResponse = context.Response;
            Stream owinResponseStream = owinResponse.Body;
            owinResponse.Body = new MemoryStream();

            string input = string.Empty;
            var httpInputStream = HttpContext.Current?.Request?.InputStream;
            if (httpInputStream.Length > 0) // can access http call
            {
                SetEventRequestHeaders(HttpContext.Current?.Request, _options.HttpTrackingEntry);
                input = HttpUtility.UrlDecode(await new StreamReader(httpInputStream).ReadToEndAsync());
                HttpContext.Current.Request.InputStream.Position = 0;
            }
            else
            {
                // owin
                var requestBuffer = new MemoryStream();
                var requestStream = new ContentStream(requestBuffer, request.Body);
                request.Body = requestStream;
                SetEventRequestHeaders(request, _options.HttpTrackingEntry);
                input = HttpUtility.UrlDecode(await WriteContentAsync(requestStream, _options.HttpTrackingEntry.RequestHeaders, _maxRecordedRequestLength));
            }

            var anonymizedInput = _options.Anonymizer.Anonymize(input, request.Uri.ToString(), Direction.In);
            // record request/input
            _options.HttpTrackingEntry.Request = anonymizedInput;

            // invoke next middleware 
            await Next.Invoke(context);

            // in case owin contains response
            if (outputCapture.CapturedData.Length == 0)
            {
                owinResponse.Body.Position = 0;
                await owinResponse.Body.CopyToAsync(owinResponseStream);
            }
            else
            {
                // in case we have  captured data from  mvc response copy it into owinResponse
                outputCapture.CapturedData.Position = 0;
                outputCapture.CapturedData.CopyTo(owinResponse.Body);
            }

            // set response headers.
            SetEventResponseHeaders(owinResponse, _options.HttpTrackingEntry);

            // record call duration
            sw.Stop();
            var elapsed = HumanizeTimespan(sw.Elapsed);
            _options.HttpTrackingEntry.CallDuration = elapsed;

            // finally  read final reponse  body 
            owinResponse.Body.Seek(0, SeekOrigin.Begin);
            var apiResponse = _options.Anonymizer.Anonymize(new StreamReader(owinResponse.Body).ReadToEnd(), request.Uri.ToString(), Direction.Out);
            _options.HttpTrackingEntry.Response = apiResponse;
            await SaveTraceAsync(_options.Context, _options.HttpTrackingEntry);
        }

        private async Task SaveTraceAsync(LogsDatabaseContext context, HttpEntry entry)
        {
            try
            {
                var trace = new Entities.Trace
                {
                    Id = Guid.NewGuid(),
                    CallDuration = entry.CallDuration,
                    CallerIdentity = entry.CallerIdentity ?? "(anonymous)",
                    ReasonPhrase = entry.ReasonPhrase,
                    RequestHeaders = ProcessHeaders(entry.RequestHeaders),
                    ResponseHeaders = ProcessHeaders(entry.ResponseHeaders),
                    RequestPayload = entry.Request,
                    RequestTimestamp = entry.CallDateTime,
                    ResponsePayload = entry.Response,
                    StatusCode = entry.StatusCode,
                    Url = entry.RequestUri.AbsolutePath,
                    Verb = entry.Verb
                };

                context.Traces.Add(trace);
                await context.SaveChangesAsync();
            }
            catch { }
        }

        private string ProcessHeaders(IHeaderDictionary headers)
        {
            return string.Join(";", headers.Select(x => $"{x.Key}={x.Value.First()}"));
        }

        /// <summary>
        /// Write content / request
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="headers"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private static async Task<string> WriteContentAsync(ContentStream stream, IDictionary<string, string[]> headers, long maxLength)
        {
            var contentType = headers.ContainsKey(ContentTypeHeader) ? headers[ContentTypeHeader][0] : null;
            return await stream.ReadContentAsync(contentType, maxLength);
        }

        /// <summary>
        /// Sets the request headers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="record"></param>
        private void SetEventRequestHeaders(IOwinRequest request, HttpEntry record)
        {
            record.Verb = request.Method;
            record.RequestUri = request.Uri;

            IDictionary<string, string[]> headers = new Dictionary<string, string[]>();
            foreach (var headerKey in request.Headers.Keys)
            {
                if (_options.RequestHeaderCaptureExceptions.Contains(headerKey)) continue;
                string headerValue = request.Headers[headerKey];
                headers.Add(headerKey, new[] { headerValue });
            }

            record.RequestHeaders = new HeaderDictionary(headers);
        }

        /// <summary>
        /// Set request headers from http request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="record"></param>
        private void SetEventRequestHeaders(HttpRequest request, HttpEntry record)
        {
            record.Verb = request.HttpMethod;
            record.RequestUri = request.Url;

            IDictionary<string, string[]> headers = new Dictionary<string, string[]>();
            foreach (var headerKey in request.Headers.AllKeys)
            {
                if (_options.RequestHeaderCaptureExceptions.Contains(headerKey)) continue;
                string headerValue = request.Headers[headerKey];
                headers.Add(headerKey, new[] { headerValue });
            }

            record.RequestHeaders = new HeaderDictionary(headers);
        }

        /// <summary>
        /// Set response headers
        /// </summary>
        /// <param name="response"></param>
        /// <param name="record"></param>
        private void SetEventResponseHeaders(IOwinResponse response, HttpEntry record)
        {
            record.StatusCode = response.StatusCode;
            record.ReasonPhrase = response.ReasonPhrase;

            IDictionary<string, string[]> headers = new Dictionary<string, string[]>();
            foreach (var headerKey in response.Headers.Keys)
            {
                if (_options.RequestHeaderCaptureExceptions.Contains(headerKey)) continue;
                string headerValue = response.Headers[headerKey];
                headers.Add(headerKey, new[] { headerValue });
            }

            record.ResponseHeaders = new HeaderDictionary(headers);
        }

        /// <summary>
        /// Display a timespan for humans
        /// </summary>
        /// <param name="elapsed"></param>
        /// <returns></returns>
        private string HumanizeTimespan(TimeSpan elapsed)
        {
            int minutes = (int)elapsed.TotalMinutes;
            double fsec = 60 * (elapsed.TotalMinutes - minutes);
            int sec = (int)fsec;
            int ms = (int)(1000 * (fsec - sec));
            string tsOut = String.Format("{0}:{1:D2}.{2}ms", minutes, sec, ms);
            return tsOut;
        }
    }
}
