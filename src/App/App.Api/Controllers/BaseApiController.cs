using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using App.Entities;
using App.Infrastructure.Security;
using Microsoft.AspNet.Identity;

namespace App.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected class UserDetails
        {
            public UserProfile User { get; set; }
        }

        protected List<Tuple<string, string>> GetUserClaims()
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity == null)
                throw new NoNullAllowedException("identity");

            var claimCollection = identity.Claims.Select(x => new Tuple<string, string>(x.Type, x.Value)).ToList();

            return claimCollection;
        }

        protected UserDetails CurrentUser
        {
            get
            {
                var applicationPrincipal = RequestContext.Principal as AppClaimsPrincipal;

                if (applicationPrincipal == null)
                    throw new Exception("Request Principal is empty");

                return new UserDetails
                {
                    User = applicationPrincipal.UserProfile
                };
            }
        }

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