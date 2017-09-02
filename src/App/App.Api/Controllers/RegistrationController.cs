using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using App.Api.Validation;
using App.Dto.Request;
using App.Dto.Response;
using App.Services.Contracts;

namespace App.Api.Controllers
{
    /// <summary>
    /// Client 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [ValidateViewModel]
    //[Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("api/registration")]
    public class RegistrationController : BaseApiController
    {
        private readonly IUserService _userService;

        public RegistrationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public HttpResponseMessage Register([FromBody] NewUserDto user)
        {
            var result = _userService.Register(user);
            if (result.Success) return Request.CreateResponse(HttpStatusCode.OK, NewUserResponse.Ok);

            return Request.CreateResponse(HttpStatusCode.BadRequest, new NewUserResponse
            {
                Success = false,
                Errors = result.Errors.Select(x => new Error {ErrorMessage = x})
            });
        }
    }
}