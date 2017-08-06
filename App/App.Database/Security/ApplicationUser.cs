using System;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentityUserClaim = App.Entities.Security.IdentityUserClaim;
using IdentityUserLogin = App.Entities.Security.IdentityUserLogin;
using IdentityUserRole = App.Entities.Security.IdentityUserRole;

namespace App.Database.Security
{
    public class ApplicationUser : IdentityUser<Guid, 
        IdentityUserLogin, 
        IdentityUserRole, 
        IdentityUserClaim>
    {
        public virtual UserProfile ProfileInfo { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}