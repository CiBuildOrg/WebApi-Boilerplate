using App.Core.Contracts;
using App.Infrastructure.Contracts;
using App.Infrastructure.Security;
using App.Infrastructure.Tracing;
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
            builder.RegisterType<SimpleTraceTerminal>().As<ITraceTerminal>().InstancePerLifetimeScope();
            builder.RegisterType<TraceProvider>().As<ITraceProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ApiLogHandler>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<RefreshTokenManager>().As<IRefreshTokenManager>().InstancePerLifetimeScope();
            builder.RegisterType<RefreshTokenProvider>().As<IAuthenticationTokenProvider>().InstancePerLifetimeScope();
            builder.RegisterType<JwtFormat>().As<ISecureDataFormat<AuthenticationTicket>>().InstancePerLifetimeScope();
            builder.RegisterType<OauthProvider>().As<OAuthAuthorizationServerProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().As<IApplicationUserManager>().InstancePerLifetimeScope();
            builder.RegisterType<IImageProcessorService>().As<IImageProcessorService>().InstancePerLifetimeScope();
        }
    }
}
