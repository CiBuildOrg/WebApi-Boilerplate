using System;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Entities;
using App.Entities.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Database.Security
{
    public class ApplicationUser : IdentityUser<Guid, 
        ApplicationIdentityUserLogin, 
        ApplicationIdentityUserRole, 
        ApplicationIdentityUserClaim>
    {
        public virtual UserProfile ProfileInfo { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}