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

            return Ok(identity.Claims.Select(c => new
            {
                c.Type,
                c.Value
            }));
        }
    }
}