using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Optimization;
using App.Exceptions;
using App.Infrastructure.Extensions;
using App.Infrastructure.Tracing;
using Autofac;
using Autofac.Integration.WebApi;

namespace App.Api
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.ConfigureContainer();
            GlobalConfiguration.Configuration.MessageHandlers.Add(GetResolver().Resolve<ApiLogHandler>());


            // monitor unhandled exceptions
            this.EnableMonitoring();
        }

        private static ILifetimeScope GetResolver()
        {
            // intiialize the container
            var dependencyComponent = GlobalConfiguration.Configuration.DependencyResolver;
            if (dependencyComponent == null)
                throw new NoDependencyInjectionSetupException("Dependency component not found");

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (!(dependencyComponent is AutofacWebApiDependencyResolver))
            {   
                throw new NoAutofacContainerFoundException("Dependency resolver is not Autofac");
            }

            return ((AutofacWebApiDependencyResolver)dependencyComponent).Container;
        }
    }
}