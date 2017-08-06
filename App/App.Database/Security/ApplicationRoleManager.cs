using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace App.Database.Security
{
    public class IdentityRoleStore : RoleStore<CustomRole, Guid, App.Entities.Security.IdentityUserRole>
    {
        public IdentityRoleStore(DatabaseContext context)
            : base(context)
        {
        }
    }

    public class ApplicationRoleManager : RoleManager<CustomRole, Guid>
    {
        public ApplicationRoleManager(IRoleStore<CustomRole, Guid> store) : base(store)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new IdentityRoleStore(context.Get<DatabaseContext>()));

            return appRoleManager;
        }
    }
}