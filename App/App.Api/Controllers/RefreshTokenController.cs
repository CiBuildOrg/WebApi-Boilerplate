using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using App.Core.Extensions;

namespace App.Api.Controllers
{
    /// <summary>
    /// Refresh Token 
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] 
    [Authorize(Roles = "SuperAdmin")]
    [RoutePrefix("api/refreshtoken")]
    public class RefreshTokenController : BaseApiController
    {
        /// <summary>
        /// Gets all refresh tokens in current WebApi
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(AppRefreshTokenManager.GetAllRefreshTokens());
        }

        /// <summary>
        /// Delete refresh token
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var hashedTokenId = tokenId.GetHash();
            var result = await AppRefreshTokenManager.RemoveRefreshToken(hashedTokenId);

            if (result)
            {
                return Ok();
            }

            return BadRequest($"Token : '{tokenId}' does not exist.");
        }
    }
}