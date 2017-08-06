using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace App.Database.Security
{
    public class UsernameValidator : UserValidator<ApplicationUser, Guid>
    {
        private readonly List<string> _allowedEmailDomains = new List<string>
        {
            "gmail.com",
            "yahoo.com",
            "hotmail.com",
            
            // add yours here
        };

        public UsernameValidator(UserManager<ApplicationUser, Guid> manager) : base(manager)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            var result = await base.ValidateAsync(user);

            var emailDomain = user.Email.Split('@')[1];

            if (_allowedEmailDomains.Contains(emailDomain.ToLower()))
                return result;

            var errors = result.Errors.ToList();
            errors.Add($"Email domain '{emailDomain}' is not allowed.");
            result = new IdentityResult(errors);

            return result;
        }
    }
}