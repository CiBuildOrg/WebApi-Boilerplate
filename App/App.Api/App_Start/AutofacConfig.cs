using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using App.Core.Contracts;
using App.Core.Implementations;
using App.Database;
using App.Infrastructure.Di;
using App.Infrastructure.Tracing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace App.Api
{
    public class AutofacConfig
    {
        private class Inner { }

        private static IContainer Container { get; set; }

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(AutofacConfig).Assembly);

            RegisterDependencies(builder);
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            RegisterModules(builder, assemblies);
            AutowireProperties(builder);
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new BusDomainModule());
            builder.Register<IResolver>(x => new Resolver(Container));

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }

        private static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleTracer>().As<ITracer>().InstancePerLifetimeScope();
            builder.RegisterType<TraceStepper>().As<ITraceStepper>().InstancePerLifetimeScope();

            builder.RegisterType<ApiLogHandler>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();

            builder.RegisterType<NowImplementation>().As<INow>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseContext>().As<IDatabaseContext>().InstancePerLifetimeScope();
            builder.RegisterType<TraceStepUtil>().As<ITraceStepUtil>().InstancePerLifetimeScope();

            builder.RegisterType<WebConfiguration>().As<IConfiguration>().SingleInstance();
        }

        private static void AutowireProperties(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(typeof(Inner).Assembly)
                .PropertiesAutowired();

            builder.RegisterType<Global>()
                .PropertiesAutowired();
        }

        private static void RegisterModules(ContainerBuilder builder, Assembly[] assemblies)
        {
            // register modules from assemblies
            builder.RegisterAssemblyModules(assemblies);
        }
    }
}