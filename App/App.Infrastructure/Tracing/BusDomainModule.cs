using Autofac;
using Enexure.MicroBus;
using Enexure.MicroBus.Autofac;

namespace App.Infrastructure.Tracing
{
    public class BusDomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var busBuilder = new BusBuilder();
            busBuilder.RegisterCommandHandler<ApiEntryCommand, ApiEntryHandler>();
            builder.RegisterMicroBus(busBuilder);
            base.Load(builder);
        }
    }
}
