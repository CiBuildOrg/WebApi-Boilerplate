using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using App.Api.ErrorHandling;
using App.Infrastructure.Utils.Multipart;
using App.Validation.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.IgnoreRoute("axd", "{resource}.axd/{*pathInfo}");

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Formatters
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            GlobalConfiguration.Configuration.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());
            GlobalConfiguration.Configuration.Filters.Add(new ExceptionFilter());
            GlobalConfiguration.Configuration.Filters.Add(new ValidationActionFilter());
            // add this to ensure webapi does not crash on unhandled exceptions. 
            config.Services.Replace(typeof(IExceptionHandler), new OopsExceptionHandler());

            config.EnsureInitialized();
        }
    }
}
