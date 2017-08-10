using System;
using System.Data.Entity;
using App.Entities.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Security.Infrastructure
{
    public class IdentityRoleStore : RoleStore<ApplicationRole, Guid, ApplicationIdentityUserRole>
    {
        public IdentityRoleStore(DbContext context)
            : base(context)
        {
        }
    }
}