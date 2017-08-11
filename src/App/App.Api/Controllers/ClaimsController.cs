using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace App.Api.Controllers
{
    [RoutePrefix("api/claims")]
    public class ClaimsController : BaseApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult GetClaims()
        {
            var identity = User.Identity as ClaimsIdentity;

            var claims = from c in identity?.Claims
                select new
                {
                    subject = c.Subject.Name,
                    type = c.Type,
                    value = c.Value
                };

            return Ok(claims);
        }

    }
}