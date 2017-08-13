using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace App.Api.Controllers
{
    /// <summary>
    /// Test Protected Controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/test")]
    public class ProtectedController : BaseApiController
    {
        /// <summary>
        /// GET method with Authentication and Authorization; 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            var identity = User.Identity as ClaimsIdentity;

            if(identity == null)
                throw new NoNullAllowedException("identity");

            var claimCollection = identity.Claims.Select(x => new {x.Type, x.Value}).ToList();

            return Ok(new
            {
                Message = $"Hello {identity.Name}",
                claimCollection
            });
        }

        [HttpGet]
        [Route("admin")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IHttpActionResult AdminOnly()
        {
            return Ok("Admin here");
        }

        [HttpGet]
        [Route("user")]
        [Authorize(Roles = "User")]
        public IHttpActionResult UserOnly()
        {
            return Ok("User here");
        }
    }
}