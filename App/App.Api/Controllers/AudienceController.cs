using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using App.Dto.Request;

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
        /// <summary>
        /// Get all clients.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetAudiences()
        {
            var lists = AppRefreshTokenManager.GetClients();

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

            var newClient = await AppRefreshTokenManager.AddClientAsync(clientModel);

            if (newClient != null)
            {
                return Ok(TheModelFactory.Create(newClient));
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
            var client = AppRefreshTokenManager.FindClient(id);
            if (client == null) return NotFound();
            var result = await AppRefreshTokenManager.RemoveClient(id);

            if (result) return Ok();
            ModelState.AddModelError("", $"Client: '{id}' could not delete.");

            return BadRequest(ModelState);
        }
    }
}