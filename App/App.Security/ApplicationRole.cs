using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Security
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name) : base(name)
        {
        }

        public string Description { get; set; }
    }
}