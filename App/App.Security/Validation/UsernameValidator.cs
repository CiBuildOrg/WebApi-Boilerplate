using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace App.Security
{
    public class UsernameValidator : UserValidator<ApplicationUser>
    {
        private readonly List<string> _allowedEmailDomains = new List<string>
        {
            "gmail.com",
            "yahoo.com",
            "hotmail.com",
            
            // add yours here
        };

        public UsernameValidator(UserManager<ApplicationUser, string> manager) : base(manager)
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