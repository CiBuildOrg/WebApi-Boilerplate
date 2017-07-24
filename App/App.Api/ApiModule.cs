using App.Core.Contracts;
using Autofac;

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