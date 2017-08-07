using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity;

namespace App.Database.Security
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole, Guid>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, Guid> store) : base(store)
        {
            
        }
    }
}