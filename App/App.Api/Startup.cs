using System.Web.Http;
using App.Api;
using App.Infrastructure.Logging.Owin;
using App.Infrastructure.Tracing;
using Autofac;
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
            GlobalConfiguration.Configuration.MessageHandlers.Add(container.Resolve<ApiLogHandler>());
            app.UseCommonLogging();
        }
    }
}