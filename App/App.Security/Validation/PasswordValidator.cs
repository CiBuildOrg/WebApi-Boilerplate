using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace App.Security.Validation
{
    public class CustomPasswordValidator : PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            var result = await base.ValidateAsync(password);

            var passwordStrength = PasswordChecker.EvalPwdStrength(password);
            if (passwordStrength == PasswordStrength.Strong
                || passwordStrength == PasswordStrength.Medium
                || passwordStrength == PasswordStrength.Normal)
            {
                return result;
            }

            var errors = result.Errors.ToList();
            errors.Add("Password is too weak");
            result = new IdentityResult(errors);

            return result;
        }
    }
}