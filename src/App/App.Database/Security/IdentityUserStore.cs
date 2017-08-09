using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Database.Security
{
    public class IdentityUserStore : UserStore<ApplicationUser, ApplicationRole, Guid,
        ApplicationIdentityUserLogin, ApplicationIdentityUserRole, ApplicationIdentityUserClaim>
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public IdentityUserStore(DatabaseContext context)
            : base(context)
        {
        }
    }
}