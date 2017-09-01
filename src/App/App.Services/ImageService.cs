using System;
using System.IO;
using System.Linq;
using System.Web;
using App.Core;
using App.Database;
using App.Dto.Response;
using App.Entities;
using App.Services.Contracts;

namespace App.Services
{
    public class ImageService : IImageService
    {
        private readonly IStorageProvider _storageProvider;
        private readonly DatabaseContext _context;
        private readonly IImageProcessorService _imageProcessorService;

        public ImageService(IStorageProvider storageProvider, DatabaseContext context, IImageProcessorService imageProcessorService)
        {
            _storageProvider = storageProvider;
            _context = context;
            _imageProcessorService = imageProcessorService;
        }

        public void StoreImage(MemoryStream image, Guid imageId)
        {
            var extension = MimeTypeMap.List.MimeTypeMap.GetExtension(ApplicationConstants.DefaultMimeType).First();
            var imagePath =
                HttpContext.Current.Server.MapPath(String.Format(ApplicationConstants.ImagePathTemplate,
                    ApplicationConstants.DefaultUserSubDirectory,
                    ReturnSizeSubdirectory(ImageSize.Small),
                    imageId, extension));

            _storageProvider.StoreFile(imagePath, image);
        }

        /// <summary>
        /// Returns the size directory ( small/medium/large )
        /// </summary>
        /// <param name="imageSize"></param>
        /// <returns></returns>
        private static string ReturnSizeSubdirectory(ImageSize imageSize)
        {
            switch (imageSize)
            {
                case ImageSize.Small:
                    return "small";
                case ImageSize.Medium:
                    return "medium";
                case ImageSize.Large:
                    return "large";
                default: throw new InvalidOperationException($"Image size {imageSize} does not exist");
            }
        }

        public ImageResponse GetImage(Guid imageId)
        {
            var image = _context.Images.SingleOrDefault(x => x.Id == imageId);

            if (image == null)
            {
                return new ImageResponse
                {
                    MimeType = ApplicationConstants.DefaultMimeType,
                    ImagePayload = _imageProcessorService.GetDefaultImage().ImagePayload
                };
            }

            var extension = MimeTypeMap.List.MimeTypeMap.GetExtension(image.MimeType).First();

            var imagePath =
                HttpContext.Current.Server.MapPath(String.Format(ApplicationConstants.ImagePathTemplate,
                    ApplicationConstants.DefaultUserSubDirectory,
                    ReturnSizeSubdirectory(ImageSize.Small),
                    imageId, extension));

            return new ImageResponse
            {
                MimeType = image.MimeType,
                ImagePayload = _storageProvider.GetFile(imagePath)
            };
        }
    }
}