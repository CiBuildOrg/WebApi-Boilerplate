using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace App.Security.Password
{
    public class PasswordValidator : Microsoft.AspNet.Identity.PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            var result = await base.ValidateAsync(password);

            // add your custom logic to this 
            if (!password.Contains("abcdef") && !password.Contains("123456"))
                return result;

            var errors = result.Errors.ToList();
            errors.Add("Password can not containe sequence of chars");
            result = new IdentityResult(errors);

            return result;
        }
    }
}