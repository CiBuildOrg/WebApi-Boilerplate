using System;
using App.Entities.Security;
using App.Security.Infrastructure;
using App.Security.Validation;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace App.Security
{
    public class SecurityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<PasswordValidator>(x => new CustomPasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            });

            builder.RegisterType<ApplicationUserManager>().As<UserManager<ApplicationUser, Guid>>().InstancePerLifetimeScope();
            builder.RegisterType<IdentityUserStore>().As<IUserStore<ApplicationUser, Guid>>();
            builder.RegisterType<IdentityRoleStore>().As<IRoleStore<ApplicationRole, Guid>>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationRoleManager>().As<RoleManager<ApplicationRole, Guid>>()
                .InstancePerLifetimeScope();

            builder.Register(x =>
                new IdentityFactoryOptions<ApplicationUserManager>
                {
                    DataProtectionProvider = new DpapiDataProtectionProvider("App.Api")
                }).InstancePerLifetimeScope();
        }
    }
}
