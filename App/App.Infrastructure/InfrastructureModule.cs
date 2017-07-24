using App.Core.Contracts;
using App.Infrastructure.Tracing;
using Autofac;
using Enexure.MicroBus;
using Enexure.MicroBus.Autofac;

namespace App.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleTracer>().As<ITracer>().InstancePerLifetimeScope();
            builder.RegisterType<TraceStepper>().As<ITraceStepper>().InstancePerLifetimeScope();
            builder.RegisterType<ApiLogHandler>().AsSelf().InstancePerLifetimeScope();

            RegisterBus(builder);
        }

        private static void RegisterBus(ContainerBuilder builder)
        {
            var busBuilder = new BusBuilder();
            busBuilder.RegisterCommandHandler<ApiEntryCommand, ApiEntryHandler>();
            builder.RegisterMicroBus(busBuilder);
        }
    }
}
