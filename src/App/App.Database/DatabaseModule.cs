using System.Data.Entity;
using Autofac;
using App.Database.LogsMigrations;

namespace App.Database
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseContext>()
                .AsSelf().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<LogsDatabaseContext>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
