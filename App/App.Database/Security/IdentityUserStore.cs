using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentityUserClaim = App.Entities.Security.IdentityUserClaim;
using IdentityUserLogin = App.Entities.Security.IdentityUserLogin;
using IdentityUserRole = App.Entities.Security.IdentityUserRole;

namespace App.Database.Security
{
    public class IdentityUserStore : UserStore<ApplicationUser, CustomRole, Guid,
        IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public IdentityUserStore(DatabaseContext context)
            : base(context)
        {
        }
    }
}