using App.Core.Contracts;
using App.Infrastructure.Tracing;
using Autofac;

namespace App.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleTraceTerminal>().As<ITraceTerminal>().InstancePerLifetimeScope();
            builder.RegisterType<TraceProvider>().As<ITraceProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ApiLogHandler>().AsSelf().InstancePerLifetimeScope();
        }

    }
}
