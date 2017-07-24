using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Contracts;
using App.Core.Implementations;
using Autofac;
using Autofac.Core;

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
