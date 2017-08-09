using App.Core.Contracts;
using App.Core.Implementations;
using Autofac;

namespace App.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NowImplementation>().As<INow>().InstancePerLifetimeScope();
            builder.RegisterType<TraceStepUtil>().As<ITraceStepUtil>().InstancePerLifetimeScope();

        }
    }
}
