using System.Data.Entity;
using App.Entities.Security;
using App.Security.Infrastructure;
using Autofac;
using Microsoft.AspNet.Identity;

namespace App.Database
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseContext>()
                .AsSelf().As<DbContext>().InstancePerLifetimeScope();

        }
    }
}
