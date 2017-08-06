using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using IdentityUserClaim = App.Entities.Security.IdentityUserClaim;
using IdentityUserLogin = App.Entities.Security.IdentityUserLogin;
using IdentityUserRole = App.Entities.Security.IdentityUserRole;

namespace App.Database.Security
{
    public class IdentityUserStorage : UserStore<ApplicationUser, CustomRole, Guid,
        IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {

        public IdentityUserStorage(DatabaseContext context)
            : base(context)
        {
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var appDbContext = context.Get<DatabaseContext>();

            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser, CustomRole,
                Guid, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(appDbContext));

            appUserManager.UserValidator = new UsernameValidator(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            return appUserManager;
        }
    }
}