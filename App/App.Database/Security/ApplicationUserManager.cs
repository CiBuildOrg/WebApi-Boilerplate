using System;
using Microsoft.AspNet.Identity;

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