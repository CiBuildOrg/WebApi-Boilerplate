using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using App.Api;
using App.Api.Security;
using App.Core.Contracts;
using App.Core.Implementations;
using App.Database.Security;
using App.Entities;
using App.Entities.Security;
using App.Infrastructure.Di;
using App.Infrastructure.Logging.Owin;
using App.Security.Infrastructure;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using JwtFormat = App.Api.Security.JwtFormat;

[assembly: OwinStartup("ProductionConfiguration", typeof(Startup))]
namespace App.Api
{
    public class Startup
    {
        /// <summary>
        /// Gets OWIN property name of allowedOrigin
        /// </summary>
        public const string ClientAllowedOriginPropertyName = "as:clientAllowedOrigin";

        /// <summary>
        /// Gets OWIN property name of refresh token life time
        /// </summary>
        public const string ClientRefreshTokenLifeTimePropertyName = "as:clientRefreshTokenLifeTime";

        /// <summary>
        /// Gets OWIN property name of audience (client id)
        /// </summary>
        public const string ClientPropertyName = "as:client_id";

        public void Configuration(IAppBuilder app)
        {
            var container = AutofacConfig.ConfigureContainer();
            app.UseAutofacMiddleware(container);
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<RefreshTokenManager>(RefreshTokenManager.Create);

            ConfigureOAuth(app);
            ConfigureOAuthTokenConsumption(app);

            app.UseCommonLogging();
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["tokenIssuer"];

            OAuthAuthorizationServerOptions serverOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/auth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new OauthProvider(),
                AccessTokenFormat = new JwtFormat(issuer),
                RefreshTokenProvider = new RefreshTokenProvider()
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

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["tokenIssuer"];

            app.UseOAuthBearerAuthentication(
                new OAuthBearerAuthenticationOptions
                {
                    AccessTokenFormat = new JwtFormat(issuer)
                });
        }

        private class AutofacConfig
        {
            private class Inner { }

            private static IContainer Container { get; set; }

            public static IContainer ConfigureContainer()
            {
                var builder = new ContainerBuilder();
                RegisterAllModules(builder);

                builder.RegisterApiControllers(typeof(AutofacConfig).Assembly);
                AutowireProperties(builder);
                builder.RegisterControllers(Assembly.GetExecutingAssembly());

                RegisterDependencies(builder);
                Container = builder.Build();

                DependencyResolver.SetResolver(new AutofacResolver(Container));
                GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);

                return Container;
            }

            private static void AutowireProperties(ContainerBuilder builder)
            {
                builder.RegisterApiControllers(typeof(Inner).Assembly).PropertiesAutowired();

                builder.RegisterType<Global>().PropertiesAutowired();
            }

            private static void RegisterAllModules(ContainerBuilder builder)
            {
                var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
                // register modules from assemblies
                builder.RegisterAssemblyModules(assemblies);
            }

            private static void RegisterDependencies(ContainerBuilder builder)
            {
                builder.Register<IResolver>(x => new Resolver(Container));
            }
        }
    }
}