using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace App.Entities.Security
{
    public class ApplicationUser : IdentityUser<Guid, 
        ApplicationIdentityUserLogin, 
        ApplicationIdentityUserRole, 
        ApplicationIdentityUserClaim>
    {
        public UserProfile ProfileInfo { get; set; }

        public virtual ICollection<RefreshToken> RefreshToken { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}