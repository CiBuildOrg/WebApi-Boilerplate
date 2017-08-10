using App.Api.Security;
using App.Core.Contracts;
using App.Security.Contracts;
using Autofac;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace App.Api
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WebConfiguration>().As<IConfiguration>().SingleInstance();
            builder.RegisterType<RefreshTokenManager>().As<IRefreshTokenManager>().InstancePerLifetimeScope();
            builder.RegisterType<RefreshTokenProvider>().As<IAuthenticationTokenProvider>().InstancePerLifetimeScope();
            builder.RegisterType<JwtFormat>().As<ISecureDataFormat<AuthenticationTicket>>().InstancePerLifetimeScope();
            builder.RegisterType<OauthProvider>().As<OAuthAuthorizationServerProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().As<IApplicationUserManager>().InstancePerLifetimeScope();
        }
    }
}