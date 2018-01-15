using System;
using App.Entities.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace App.Security.Infrastructure
{
    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store, 
            PasswordValidator passwordValidator, IdentityFactoryOptions<ApplicationUserManager> factory) 
            : base(store)
        {
            
            UserValidator = new UsernameValidator(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            PasswordValidator = passwordValidator;
            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, Guid>(factory.DataProtectionProvider.Create("ASP NET Identity"));
        }
    }
}