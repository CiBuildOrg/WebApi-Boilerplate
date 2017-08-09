using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using App.Api.Models;
using App.Api.Security;
using App.Database.Security;
using App.Dto.Request;
using Microsoft.AspNet.Identity;

namespace App.Api.Controllers
{
    /// <summary>
    /// Client 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] 
    [Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("api/audience")]
    public class AudienceController : BaseApiController
    {
        private readonly IRefreshTokenManager _refreshTokenManager;
        private readonly UserManager<ApplicationUser, Guid> _applicationUserManager;
        private readonly ModelFactory _factory;
        public AudienceController(IRefreshTokenManager refreshTokenManager, UserManager<ApplicationUser, Guid> applicationUserManager)
        {
            _refreshTokenManager = refreshTokenManager;
            _applicationUserManager = applicationUserManager;
            _factory = new ModelFactory(_applicationUserManager);
        }

        /// <summary>
        /// Get all clients.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetAudiences()
        {
            var lists = _refreshTokenManager.GetClients();

            return Ok(lists);
        }

        /// <summary>
        /// Create new client.
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateClient([FromBody] ClientBindingModel clientModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newClient = await _refreshTokenManager.AddClientAsync(clientModel);

            if (newClient != null)
            {
                return Ok(_factory.Create(newClient));
            }

            ModelState.AddModelError("", $"Client: '{clientModel.Name}' could not be added to clients.");

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:guid}", Name = "DeleteClient")]
        public async Task<IHttpActionResult> DeleteClient(string id)
        {
            var client = _refreshTokenManager.FindClient(id);
            if (client == null) return NotFound();
            var result = await _refreshTokenManager.RemoveClient(id);

            if (result) return Ok();
            ModelState.AddModelError("", $"Client: '{id}' could not delete.");

            return BadRequest(ModelState);
        }
    }
}