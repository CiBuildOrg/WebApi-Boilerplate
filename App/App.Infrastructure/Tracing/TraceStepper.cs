using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using App.Core;
using App.Core.Contracts;
using App.Core.Utils;
using App.Dto.Traces;
using Enexure.MicroBus;
using Newtonsoft.Json;
using ProductionStackTrace;

namespace App.Infrastructure.Tracing
{
    public class TraceStepper : ITraceStepper
    {
        private readonly ITracer _tracer;
        private readonly IConfiguration _helper;

        public TraceStepper(ITracer tracer, IConfiguration helper)
        {
            _tracer = tracer;
            _helper = helper;
        }

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldLogSteps);

        public void WriteMessage(string message)
        {
            if (ShouldLog)
            {
                var stackTrace = new StackTrace();

                var stackTraceInfo = StackTraceParser.Parse(
                    stackTrace.ToString(),
                    (f, t, m, pl, ps, fn, ln) => new
                    {
                        Frame = f,
                        Type = t,
                        Method = m,
                        ParameterList = pl,
                        Parameters = ps,
                        File = fn,
                        Line = ln,
                    });


                string methodName = null;
                string frame = null;

                bool @break = false;
                foreach (var stackTraceEntry in stackTraceInfo)
                {
                    if (@break)
                    {
                        methodName = stackTraceEntry.Method;
                        frame = stackTraceEntry.Frame;

                        break;
                    }

                    var type = stackTraceEntry.Type;
                    if (string.IsNullOrEmpty(type)) continue;

                    var @typeof = typeof(TraceStepper).Name;
                    if (type.EndsWith(@typeof))
                    {
                        @break = true;
                    }
                }


                _tracer.WriteMessage(methodName, frame, message);
            }
        }

        public void WriteException(Exception exception, string description)
        {
            if (ShouldLog)
            {
                var stackTrace = new StackTrace();

                var stackTraceInfo = StackTraceParser.Parse(
                    stackTrace.ToString(),
                    (f, t, m, pl, ps, fn, ln) => new
                    {
                        Frame = f,
                        Type = t,
                        Method = m,
                        ParameterList = pl,
                        Parameters = ps,
                        File = fn,
                        Line = ln,
                    });
                string methodName = null;
                string frame = null;

                bool @break = false;
                foreach (var stackTraceEntry in stackTraceInfo)
                {
                    if (@break)
                    {
                        methodName = stackTraceEntry.Method;
                        frame = stackTraceEntry.Frame;

                        break;
                    }

                    var type = stackTraceEntry.Type;
                    if (string.IsNullOrEmpty(type)) continue;

                    var @typeof = typeof(TraceStepper).Name;
                    if (type.EndsWith(@typeof))
                    {
                        @break = true;
                    }
                }

                var exceptionName = exception.GetType().Name;
                var exceptionTrace = ExceptionReporting.GetExceptionReport(exception);
                _tracer.WriteException(methodName, frame, exceptionTrace, description, exceptionName);
            }
        }

        public void WriteOperation(string description, string name, string operationMetadata)
        {
            if (ShouldLog)
            {
                var stackTrace = new StackTrace();

                var stackTraceInfo = StackTraceParser.Parse(
                    stackTrace.ToString(),
                    (f, t, m, pl, ps, fn, ln) => new
                    {
                        Frame = f,
                        Type = t,
                        Method = m,
                        ParameterList = pl,
                        Parameters = ps,
                        File = fn,
                        Line = ln,
                    });

                string methodName = null;
                string frame = null;

                bool @break = false;
                foreach (var stackTraceEntry in stackTraceInfo)
                {
                    if (@break)
                    {
                        methodName = stackTraceEntry.Method;
                        frame = stackTraceEntry.Frame;

                        break;
                    }

                    var type = stackTraceEntry.Type;
                    if (string.IsNullOrEmpty(type)) continue;

                    var @typeof = typeof(TraceStepper).Name;
                    if (type.EndsWith(@typeof))
                    {
                        @break = true;
                    }
                }

                _tracer.WriteOperation(methodName, frame, description, name, operationMetadata);
            }
        }

        public void Dispose()
        {
        }
    }

    public class ApiLogHandler : DelegatingHandler
    {
        private readonly ITracer _tracer;
        private readonly ITraceStepper _traceStepper;
        private readonly IMicroBus _bus;
        private readonly IConfiguration _helper;

        public ApiLogHandler(ITracer tracer, ITraceStepper traceStepper, IMicroBus bus, IConfiguration helper)
        {
            _tracer = tracer;
            _traceStepper = traceStepper;
            _bus = bus;
            _helper = helper;
        }

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldLogSteps);

        private void ProcessRequest(Task<string> task, ApiLogEntry apiLogEntry)
        {
            if (!ShouldLog) return;

            apiLogEntry.RequestContentBody = task.Result;

            if (string.IsNullOrEmpty(apiLogEntry.RequestContentBody))
            {
                apiLogEntry.RequestContentBody = "No body payload detected";
            }

            _traceStepper.WriteOperation("Web API request", "request headers", apiLogEntry.RequestHeaders);
            _traceStepper.WriteOperation("Web API request", "query string", apiLogEntry.RequestUri);
            _traceStepper.WriteOperation("Web API request", "body request", apiLogEntry.RequestContentBody);
        }

        private async Task<HttpResponseMessage> ProcessResponse(Task<HttpResponseMessage> task, ApiLogEntry apiLogEntry)
        {
            var response = task.Result;

            if (!ShouldLog) return response;

            // Update the API log entry with response info
            apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
            apiLogEntry.ResponseTimestamp = DateTime.Now;

            if (response.Content != null)
            {
                apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
            }

            // TODO: Save the API log entry to the database

            _traceStepper.WriteOperation("Web API response", "response body", apiLogEntry.ResponseContentBody);
            _traceStepper.WriteOperation("Web API response", "response headers", apiLogEntry.ResponseHeaders);

            _traceStepper.Dispose();

            var traceSteps = _tracer.TraceSteps;
            _tracer.Clear();

            await _bus.SendAsync(new ApiEntryCommand(apiLogEntry, traceSteps));

            return response;
        }



        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            ApiLogEntry apiLogEntry = null;

            if (ShouldLog)
            {
                apiLogEntry = CreateApiLogEntryWithRequestData(request);
            }

            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        ProcessRequest(task, apiLogEntry);
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken).ContinueWith(task => AsyncHelper.RunSync(() => ProcessResponse(task, apiLogEntry)), cancellationToken);
        }

        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();

            return new ApiLogEntry
            {
                Application = "Web API",
                User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                RequestRouteTemplate = routeData.Route.RouteTemplate,
                RequestRouteData = SerializeRouteData(routeData),
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private static string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value == null) continue;

                var header = item.Value.Aggregate(String.Empty, (current, value) => current + (value + " "));
                // Trim the trailing space and add item to the dictionary
                header = header.TrimEnd(" ".ToCharArray());
                dict.Add(item.Key, header);
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }
}
