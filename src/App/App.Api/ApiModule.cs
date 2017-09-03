using System.Web;
using System.Web.Hosting;
using System.Web.Http.Routing;
using System.Web.Routing;
using App.Core.Contracts;
using Autofac;

namespace App.Api
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WebConfiguration>().As<IConfiguration>().SingleInstance();

            builder.Register(c => new HttpContextWrapper(HttpContext.Current))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpContextBase>().Application)
                .As<HttpApplicationStateBase>()
                .InstancePerLifetimeScope();

            // HttpRequest properties
            builder.Register(c => c.Resolve<HttpRequestBase>().Browser)
                .As<HttpBrowserCapabilitiesBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpRequestBase>().Files)
                .As<HttpFileCollectionBase>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<HttpRequestBase>().RequestContext)
                .As<RequestContext>()
                .InstancePerLifetimeScope();

            // HttpResponse properties
            builder.Register(c => c.Resolve<HttpResponseBase>().Cache)
                .As<HttpCachePolicyBase>()
                .InstancePerLifetimeScope();

            // HostingEnvironment properties
            builder.Register(c => HostingEnvironment.VirtualPathProvider)
                .As<VirtualPathProvider>()
                .InstancePerLifetimeScope();

            // MVC types
            builder.Register(c => new System.Web.Mvc.UrlHelper(c.Resolve<RequestContext>()))
                .As<UrlHelper>()
                .InstancePerLifetimeScope();
        }
    }
}