using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Database.Security
{
    public class IdentityRoleStore : RoleStore<ApplicationRole, Guid, Entities.Security.ApplicationIdentityUserRole>
    {
        public IdentityRoleStore(DatabaseContext context)
            : base(context)
        {
        }
    }
}