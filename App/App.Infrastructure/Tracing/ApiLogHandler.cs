﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using App.Core;
using App.Core.Contracts;
using App.Dto.Traces;
using Enexure.MicroBus;
using Newtonsoft.Json;

namespace App.Infrastructure.Tracing
{
    public class ApiLogHandler : DelegatingHandler
    {
        private static bool ShouldLog(IConfiguration configuration)
        {
            return configuration.GetBool(ConfigurationKeys.ShouldTrace);
        }

        private static void ProcessRequest(string request, ApiLogEntry apiLogEntry, IConfiguration configuration, ITraceProvider traceProvider)
        {
            if (!ShouldLog(configuration)) return;

            apiLogEntry.RequestContentBody = request;

            if (string.IsNullOrEmpty(apiLogEntry.RequestContentBody))
            {
                apiLogEntry.RequestContentBody = "No body payload detected";
            }

            traceProvider.WriteOperation("Web API request", "request headers", apiLogEntry.RequestHeaders);
            traceProvider.WriteOperation("Web API request", "query string", apiLogEntry.RequestUri);
            traceProvider.WriteOperation("Web API request", "body request", apiLogEntry.RequestContentBody);
        }

        private static async Task ProcessResponse(HttpResponseMessage response, ApiLogEntry apiLogEntry, ITraceProvider traceProvider, ITraceTerminal traceTerminal, IMicroBus bus, IConfiguration configuration)
        {

            if (!ShouldLog(configuration)) return;

            // Update the API log entry with response info
            apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
            apiLogEntry.ResponseTimestamp = DateTime.Now;

            if (response.Content != null)
            {
                apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
            }

            traceProvider.WriteOperation("Web API response", "response body", apiLogEntry.ResponseContentBody);
            traceProvider.WriteOperation("Web API response", "response headers", apiLogEntry.ResponseHeaders);

            traceProvider.Dispose();

            var traceSteps = traceTerminal.TraceSteps;
            await bus.SendAsync(new ApiEntryCommand(apiLogEntry, traceSteps));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var scope = request.GetDependencyScope();

            var tracer = (ITraceTerminal)scope.GetService(typeof(ITraceTerminal));
            var traceStepper = (ITraceProvider)scope.GetService(typeof(ITraceProvider));
            var bus = (IMicroBus)scope.GetService(typeof(IMicroBus));
            var configurationHelper = (IConfiguration)scope.GetService(typeof(IConfiguration));

            ApiLogEntry apiLogEntry = null;

            if (ShouldLog(configurationHelper))
            {
                apiLogEntry = CreateApiLogEntryWithRequestData(request);
            }

            if (request.Content != null)
            {
                var requestContent = await request.Content.ReadAsStringAsync();
                ProcessRequest(requestContent, apiLogEntry, configurationHelper, traceStepper);
            }

            var response = await base.SendAsync(request, cancellationToken);
            await ProcessResponse(response, apiLogEntry, traceStepper, tracer, bus, configurationHelper);

            return response;
        }

        private static ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var context = (HttpContextBase)request.Properties["MS_HttpContext"];
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

        private static string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented);
        }

        private static string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value == null) continue;

                var header = item.Value.Aggregate(string.Empty, (current, value) => current + (value + " "));
                // Trim the trailing space and add item to the dictionary
                header = header.TrimEnd(" ".ToCharArray());
                dict.Add(item.Key, header);
            }

            return JsonConvert.SerializeObject(dict, Formatting.Indented);
        }
    }
}