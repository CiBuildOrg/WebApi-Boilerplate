using System.Security.Claims;
using System.Threading.Tasks;
using App.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Security
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile ProfileInfo { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}