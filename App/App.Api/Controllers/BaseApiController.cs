using System.Net.Http;
using System.Web.Http;
using App.Api.Models;
using App.Api.Security;
using App.Database.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace App.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected ApplicationUserManager AppUserManager => 
            Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        protected ApplicationRoleManager AppRoleManager => 
            Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        protected RefreshTokenManager AppRefreshTokenManager => 
            Request.GetOwinContext().GetUserManager<RefreshTokenManager>();

        protected ModelFactory TheModelFactory => 
            new ModelFactory(Request, AppUserManager, AppRoleManager);

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.Succeeded) return null;
            if (result.Errors == null) return null;
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            if (ModelState.IsValid)
            {
                return BadRequest();
            }

            return BadRequest(ModelState);
        }
    }
}