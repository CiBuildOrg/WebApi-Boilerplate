using System;
using App.Database.Security;
using App.Entities.Security;
using Autofac;
using Microsoft.AspNet.Identity;

namespace App.Database
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DatabaseContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().As<UserManager<ApplicationUser, Guid>>().InstancePerLifetimeScope();
            builder.RegisterType<IdentityUserStore>().As<IUserStore<ApplicationUser, Guid >> ();
            builder.RegisterType<IdentityRoleStore>().As<IRoleStore<CustomRole, Guid >> ().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationRoleManager>().As<RoleManager<CustomRole, Guid>>()
                .InstancePerLifetimeScope();
        }   
    }
}
