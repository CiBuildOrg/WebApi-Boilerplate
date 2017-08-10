using System;
using App.Api;
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
            ConfigureOAuth(app, container);
            ConfigureOAuthTokenConsumption(app, container);
            app.UseCommonLogging();
        }

        private static void ConfigureOAuth(IAppBuilder app, IComponentContext componentContext)
        {
            //var issuer = ConfigurationManager.AppSettings["tokenIssuer"];

            OAuthAuthorizationServerOptions serverOptions = new OAuthAuthorizationServerOptions
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