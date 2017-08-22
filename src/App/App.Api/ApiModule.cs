using App.Core.Contracts;
using App.Infrastructure.Contracts;
using App.Infrastructure.Security;
using Autofac;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace App.Api
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WebConfiguration>().As<IConfiguration>().SingleInstance();
            
        }
    }
}