using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using App.Api;
using App.Api.Security;
using App.Infrastructure.Logging.Owin;
using Autofac;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;

using System.Web.Routing;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using App.Infrastructure.Utils.Multipart;
using App.Api.ErrorHandling;
using App.Validation.Infrastructure;
using System.Web.Optimization;
using System.Web.Http.ExceptionHandling;

[assembly: OwinStartup("ProductionConfiguration", typeof(Startup))]
namespace App.Api
{
    public class Startup
    {
        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }

            filters.Add(new HandleErrorAttribute());
        }

        private static void RegisterMvcAreas() => AreaRegistration.RegisterAllAreas();

        private static void RegisterRoutes(RouteCollection routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private static void RegisterWebApi(HttpConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

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

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        private static void RegisterBundles(BundleCollection bundles)
        {
            if (bundles == null)
            {
                throw new ArgumentNullException(nameof(bundles));
            }

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/scripts/bootstrap-table/bootstrap-table.min.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/prettyprintjs").Include("~/Scripts/prettify.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-table.min.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/xmldisplaycss").Include("~/Content/xml.css"));
            bundles.Add(new StyleBundle("~/Content/prettyprint").Include("~/Content/prettify.css"));
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void Configuration(IAppBuilder app)
        {
            RegisterMvcAreas();
            GlobalConfiguration.Configure(RegisterWebApi);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);

            var httpConfiguration = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();

            var container = AutofacConfig.ConfigureContainer();
            app.UseAutofacMiddleware(container);
            ConfigureOAuth(app, container);
            ConfigureOAuthTokenConsumption(app, container);
            app.UseCommonLogging();
        }

        private static void ConfigureOAuth(IAppBuilder app, ILifetimeScope componentContext)
        {
            var serverOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/auth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                
                Provider = componentContext.Resolve<OAuthAuthorizationServerProvider>(),
                AccessTokenFormat = componentContext.Resolve<ISecureDataFormat<AuthenticationTicket>>(),
                RefreshTokenProvider = componentContext.Resolve<IAuthenticationTokenProvider>(),
                AccessTokenProvider = new AuthenticationTokenProvider()
            };

            app.UseOAuthAuthorizationServer(serverOptions);

            //var audience = ConfigurationManager.AppSettings["resourceApiClientId"];
            //var secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["resourceApiClientSecret"]);

            //app.UseJwtBearerAuthentication(
            //    new JwtBearerAuthenticationOptions
            //    {
            //        AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
            //        AllowedAudiences = new[] { audience },
            //        IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
            //        {
            //            new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
            //        }
            //    });
        }

        private static void ConfigureOAuthTokenConsumption(IAppBuilder app, IComponentContext componentContext)
        {
            app.UseOAuthBearerAuthentication(
                new OAuthBearerAuthenticationOptions
                {
                    AccessTokenFormat = componentContext.Resolve<ISecureDataFormat<AuthenticationTicket>>()
                });
        }
    }
}