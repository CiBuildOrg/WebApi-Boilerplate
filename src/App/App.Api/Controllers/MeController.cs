using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using App.Services.Contracts;
using App.Validation.Infrastructure;

namespace App.Api.Controllers
{
    /// <summary>
    /// Client 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [ValidateViewModel]
    //[Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("api/me")]
    public class MeController : BaseApiController
    {
        private readonly IUserService _userService;

        public MeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var user = CurrentUser;
            var id = user.User.Id;

            return Request.CreateResponse(HttpStatusCode.OK, _userService.GetUser(id));
        }
    }
}