using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using App.Services.Contracts;

namespace App.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize(Roles = "SuperAdmin,Admin,User")]
    [RoutePrefix("api/images")]
    public class ImageController : BaseApiController
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] Guid id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _imageService.GetImage(id));
        }
    }
}