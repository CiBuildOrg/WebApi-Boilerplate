using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Database.Security
{
    public class IdentityRoleStore : RoleStore<CustomRole, Guid, Entities.Security.IdentityUserRole>
    {
        public IdentityRoleStore(DatabaseContext context)
            : base(context)
        {
        }
    }
}