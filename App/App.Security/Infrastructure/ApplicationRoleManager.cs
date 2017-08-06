using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace App.Security.Infrastructure
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {   
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store) : base(store)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));

            return appRoleManager;
        }
    }
}