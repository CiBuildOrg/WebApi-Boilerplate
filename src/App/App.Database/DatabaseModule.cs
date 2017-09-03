using System.Data.Entity;
using Autofac;

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
