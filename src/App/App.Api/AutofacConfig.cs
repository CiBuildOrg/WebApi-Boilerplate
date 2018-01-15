using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using App.Api.Security;
using App.Core.Contracts;
using App.Core.Implementations;
using App.Infrastructure.Di;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace App.Api
{
    public class  AutofacConfig
    {
        private class Inner { }

        private static IContainer Container { get; set; }

        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            RegisterAllModules(builder);

            builder.RegisterApiControllers(typeof(AutofacConfig).Assembly);
            AutowireProperties(builder);
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
            return Container;
        }

        private static void AutowireProperties(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(typeof(Inner).Assembly).PropertiesAutowired();
            builder.RegisterType<Global>().PropertiesAutowired();
        }

        private static void RegisterAllModules(ContainerBuilder builder)
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            // register modules from assemblies
            builder.RegisterAssemblyModules(assemblies);
        }
    }
}