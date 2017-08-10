using System;
using System.Data.Entity;
using App.Entities.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Security.Infrastructure
{
    public class IdentityUserStore : UserStore<ApplicationUser, ApplicationRole, Guid,
        ApplicationIdentityUserLogin, ApplicationIdentityUserRole, ApplicationIdentityUserClaim>
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public IdentityUserStore(DbContext context)
            : base(context)
        {
        }
    }
}