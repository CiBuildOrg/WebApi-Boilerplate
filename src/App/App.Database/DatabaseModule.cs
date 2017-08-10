using System;
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
            builder.RegisterType<ApplicationUserManager>().As<UserManager<ApplicationUser, Guid>>().InstancePerLifetimeScope();
            builder.RegisterType<IdentityUserStore>().As<IUserStore<ApplicationUser, Guid >> ();
            builder.RegisterType<IdentityRoleStore>().As<IRoleStore<ApplicationRole, Guid >> ().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationRoleManager>().As<RoleManager<ApplicationRole, Guid>>()
                .InstancePerLifetimeScope();
        }   
    }
}
