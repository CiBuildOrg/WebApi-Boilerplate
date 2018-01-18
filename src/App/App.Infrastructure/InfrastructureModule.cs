using App.Core.Contracts;
using App.Infrastructure.Contracts;
using App.Infrastructure.Security;
using App.Infrastructure.Tracing.Middleware;
using Autofac;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace App.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RefreshTokenManager>().As<IRefreshTokenManager>().InstancePerLifetimeScope();
            builder.RegisterType<RefreshTokenProvider>().As<IAuthenticationTokenProvider>().InstancePerLifetimeScope();
            builder.RegisterType<JwtFormat>().As<ISecureDataFormat<AuthenticationTicket>>().InstancePerLifetimeScope();
            builder.RegisterType<OauthProvider>().As<IOAuthAuthorizationServerProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().As<IApplicationUserManager>().InstancePerLifetimeScope();
            builder.RegisterType<LogBookAnonymizer>().As<ILogBookAnonymizer>().InstancePerLifetimeScope();
        }
    }
}
