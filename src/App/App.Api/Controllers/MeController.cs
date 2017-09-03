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
            return Request.CreateResponse(HttpStatusCode.OK, _userService.GetUser(CurrentUser.User.Id));
        }
    }
}