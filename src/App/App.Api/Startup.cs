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
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using App.Infrastructure.Security;
using App.Entities.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Database;
using Autofac.Integration.WebApi;
using System.Reflection;
using Autofac.Integration.Mvc;
using System.Web.Compilation;
using System.Linq;
using App.Infrastructure.Di;
using System.Collections.Generic;
using App.Core;

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
            ConfigureContainer(builder, httpConfiguration);

            builder.RegisterInstance(app).As<IAppBuilder>();
            RegisterCors(builder);
            RegisterOauthConcepts(builder);
            RegisterClaimsTransform(builder);
            RegisterPathsProtection(builder);

            var container = builder.Build();
            var dependencyResolver = new AutofacWebApiDependencyResolver(container);
            httpConfiguration.DependencyResolver = dependencyResolver;
            DependencyResolver.SetResolver(new AutofacResolver(container));

            app.UseAutofacLifetimeScopeInjector(container);
            app.UseMiddlewareFromContainer<CorsMiddleware>();
            


            app.UseMiddlewareFromContainer<OAuthAuthorizationServerMiddleware>();
            app.UseMiddlewareFromContainer<CookieAuthenticationMiddleware>();
            app.UseMiddlewareFromContainer<OAuthBearerAuthenticationMiddleware>();
            app.UseMiddlewareFromContainer<ClaimsTransformationMiddleware>();
            app.UseMiddlewareFromContainer<ProtectionMiddleware>();
            app.UseCommonLogging();

            app.UseCommonLogging();
        }

        public static void ConfigureContainer(ContainerBuilder builder, HttpConfiguration configuration)
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            // register modules from assemblies
            builder.RegisterAssemblyModules(assemblies);
            builder.RegisterApiControllers(typeof(Startup).Assembly);
            builder.RegisterApiControllers(typeof(Startup).Assembly).PropertiesAutowired();
            builder.RegisterType<Global>().PropertiesAutowired();
            builder.RegisterInstance(configuration);
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
        }

        private void RegisterClaimsTransform(ContainerBuilder builder)
        {
            builder.Register(x => new ClaimsTransformationMiddlewareOptions
            {
                Context = x.Resolve<DatabaseContext>(),
                GetClaim = y => y.Authentication.User,
                NameIdentifier = ClaimTypes.NameIdentifier
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<ClaimsTransformationMiddleware>().AsSelf().InstancePerLifetimeScope();
        }

        private void RegisterCors(ContainerBuilder builder)
        {
            builder.RegisterType<CorsMiddleware>().AsSelf().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Protect swagger pages
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterPathsProtection(ContainerBuilder builder)
        {
            // register swagger protection middleware options
            builder.Register(x => new ProtectionMiddlewareOptions
            {
                Configs = new List<ProtectionConfig>
                {
                    new ProtectionConfig
                    {
                        AllowedRoles = new List<string> { Roles.SuperAdmin },
                        ProtectPath = Logs.LogsRelativePath,
                        RedirectUrl = IdentityAuthentication.LoginRelativePath
                    },
                }
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<ProtectionMiddleware>().AsSelf().InstancePerLifetimeScope();
        }

        private void RegisterOauthConcepts(ContainerBuilder builder)
        {
            builder.Register(x =>

                    new OAuthAuthorizationServerOptions
                    {
                        AllowInsecureHttp = true,
                        TokenEndpointPath = new PathString("/auth/token"),
                        AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                        Provider = x.Resolve<IOAuthAuthorizationServerProvider>(),
                        AccessTokenFormat = x.Resolve<ISecureDataFormat<AuthenticationTicket>>(),
                        RefreshTokenProvider = x.Resolve<IAuthenticationTokenProvider>(),
                        AccessTokenProvider = new AuthenticationTokenProvider()
                    }

                ).AsSelf().InstancePerLifetimeScope();

            builder.Register(x =>

                new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    Provider = new CookieAuthenticationProvider
                    {
                        // Enables the application to validate the security stamp when the user logs in.
                        // This is a security feature which is used when you change a password or add an external login to your account.  
                        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<App.Security.Infrastructure.ApplicationUserManager, ApplicationUser, Guid>(
                                                    validateInterval: TimeSpan.FromMinutes(30),
                                                    regenerateIdentityCallback: RegenerateIdentityCallback,
                                                    getUserIdCallback: GetUserIdCallback
                        ),

                    }

                }).AsSelf().InstancePerLifetimeScope();

            builder.Register(x => new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = x.Resolve<ISecureDataFormat<AuthenticationTicket>>()
            }).AsSelf().InstancePerLifetimeScope();

            builder
                .RegisterType<OAuthAuthorizationServerMiddleware>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<CookieAuthenticationMiddleware>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<OAuthBearerAuthenticationMiddleware>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }

        private Guid GetUserIdCallback(ClaimsIdentity id)
        {
            return Guid.Parse(id.GetUserId());
        }

        private async Task<ClaimsIdentity> RegenerateIdentityCallback(App.Security.Infrastructure.ApplicationUserManager manager, ApplicationUser user)
        {
            return await user.GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}