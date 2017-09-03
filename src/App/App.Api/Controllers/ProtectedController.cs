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
            var currentUser = CurrentUser;
            return Ok(new
            {
                Message = $"Hello {currentUser.User.FullName}",
                UserClaims = GetUserClaims()
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