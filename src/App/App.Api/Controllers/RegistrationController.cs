using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using App.Dto.Request;
using App.Dto.Response;
using App.Services.Contracts;

namespace App.Api.Controllers
{
    /// <summary>
    /// Client 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
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
            try
            {
                _userService.Register(user);
                return Request.CreateResponse(HttpStatusCode.OK,new NewUserResponse
                {
                    Error = null,
                    Success = true
                });
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new NewUserResponse {Success = false, Error = ex.Message});
            }

        }
    }
}