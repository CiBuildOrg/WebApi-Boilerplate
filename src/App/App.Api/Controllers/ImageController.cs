using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using App.Services.Contracts;

namespace App.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix("api/images")]
    public class ImageController : BaseApiController
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet, Route("default")]
        public HttpResponseMessage GetDefault()
        {
            var imageResponse = _imageService.GetDefaultImage();

            // serve the payload
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(imageResponse.ImagePayload)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue(imageResponse.MimeType);
            return result;
        }

        [HttpGet]
        [Route("{id:guid}", Name = "Get")]
        public HttpResponseMessage Get(Guid id)
        {
            var imageResponse = _imageService.GetImage(id);

            // serve the payload
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(imageResponse.ImagePayload)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue(imageResponse.MimeType);
            return result;
        }
    }
}