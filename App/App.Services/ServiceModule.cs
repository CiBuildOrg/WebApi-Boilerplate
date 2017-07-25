using System.Linq;
using Autofac;

namespace App.Services
{
    public class ServiceModule : Module
    {
        private const string ServicesEnding = "Service";

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsClass && t.Name.EndsWith(ServicesEnding))
                .As(t => t.GetInterfaces().Single(i => i.Name.EndsWith(t.Name))).InstancePerLifetimeScope();
        }
    }
}
