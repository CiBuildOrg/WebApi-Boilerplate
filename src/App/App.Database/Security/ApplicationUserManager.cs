using System;
using Microsoft.AspNet.Identity;

namespace App.Database.Security
{
    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store, 
            PasswordValidator passwordValidator) 
            : base(store)
        {
            UserValidator = new UsernameValidator(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            PasswordValidator = passwordValidator;
        }
    }
}