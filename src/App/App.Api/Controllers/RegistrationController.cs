using System.Web.Http;
using System.Web.Http.Description;

namespace App.Api.Controllers
{
    /// <summary>
    /// Client 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("api/registration")]
    public class RegistrationController : BaseApiController
    {

    }
}