using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
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

[assembly: OwinStartup("ProductionConfiguration", typeof(Startup))]
namespace App.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = AutofacConfig.ConfigureContainer();
            app.UseAutofacMiddleware(container);
            ConfigureOAuth(app, container);
            ConfigureOAuthTokenConsumption(app, container);
            app.UseCommonLogging();

            GlobalConfiguration.Configuration.MessageHandlers.Add(container.Resolve<CustomAuthenticationMessageHandler>());

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