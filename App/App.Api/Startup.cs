using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using App.Api;
using App.Core.Contracts;
using App.Core.Implementations;
using App.Infrastructure.Di;
using App.Infrastructure.Logging.Owin;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup("ProductionConfiguration", typeof(Startup))]
namespace App.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = AutofacConfig.ConfigureContainer();
            app.UseAutofacMiddleware(container);
            
            app.UseCommonLogging();
        }

        private class AutofacConfig
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

                RegisterDependencies(builder);
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

            private static void RegisterDependencies(ContainerBuilder builder)
            {
                builder.Register<IResolver>(x => new Resolver(Container));
            }
        }
    }
}