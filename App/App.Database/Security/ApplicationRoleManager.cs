using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity;

namespace App.Database.Security
{
    public class ApplicationRoleManager : RoleManager<CustomRole, Guid>
    {
        public ApplicationRoleManager(IRoleStore<CustomRole, Guid> store) : base(store)
        {
            
        }
    }
}