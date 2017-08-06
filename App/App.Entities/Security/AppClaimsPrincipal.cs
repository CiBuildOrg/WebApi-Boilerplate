using System.Security.Claims;
using System.Security.Principal;

namespace App.Entities.Security
{
    public class AppClaimsPrincipal : ClaimsPrincipal
    {
        public AppClaimsPrincipal(IPrincipal principal) : base(principal)
        {
        }

        public int UserId => int.Parse(FindFirst(ClaimTypes.Sid).Value);
    }
}