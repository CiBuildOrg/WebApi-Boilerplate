using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentityUserClaim = App.Entities.Security.IdentityUserClaim;
using IdentityUserLogin = App.Entities.Security.IdentityUserLogin;
using IdentityUserRole = App.Entities.Security.IdentityUserRole;

namespace App.Database.Security
{
    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store, 
            IIdentityValidator<ApplicationUser> userValidator, 
            PasswordValidator passwordValidator) 
            : base(store)
        {
            UserValidator = userValidator;
            PasswordValidator = passwordValidator;
        }
    }
}