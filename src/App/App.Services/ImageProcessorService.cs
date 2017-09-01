using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using App.Core;
using App.Dto.Response;
using App.Entities;
using App.Services.Contracts;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;

namespace App.Services
{
    public class ImageProcessorService : IImageProcessorService
    {
        private readonly IStorageProvider _storageProvider;
        


        public ImageProcessorService(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public MemoryStream ProcessAvatar(byte[] image)
        {
            using (var inStream = new MemoryStream(image))
            {
                var outStream = new MemoryStream();
                using (var imageFactory = new ImageFactory(true, true))
                {
                    // Load, resize, set the format and quality and save an image.
                    imageFactory.Load(inStream);
                    imageFactory.Format(new JpegFormat());
                    imageFactory.Quality(ApplicationConstants.AvatarSmallQualityPercentage);
                    imageFactory.Resize(new ResizeLayer(new Size(ApplicationConstants.AvatarSmallW, ApplicationConstants.AvatarSmallH)));
                    imageFactory.Save(outStream);

                    return outStream;
                }
            }
        }

        public ImageResponse GetDefaultImage()
        {
            var defaultImagePath =
                HttpContext.Current.Server.MapPath(string.Format(ApplicationConstants.ImagePathTemplate,
                    ApplicationConstants.DefaultUserSubDirectory,
                    ReturnSizeSubdirectory(ImageSize.Small),
                    "default",
                    GetImageExtension(ApplicationConstants.DefaultMimeType)));

            if (!File.Exists(defaultImagePath))
            {
                throw new FileNotFoundException("Default avatar image not found");
            }

            // load the image from the file storage ( implemented as a naive directory-based store )
            var imagePayload = _storageProvider.GetFile(defaultImagePath);

            return new ImageResponse
            {
                MimeType = ApplicationConstants.DefaultMimeType,
                ImagePayload = imagePayload,
            };
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

        /// <summary>
        /// Gets the image extension
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static string GetImageExtension(string mimeType)
        {
            return MimeTypeMap.List.MimeTypeMap.GetExtension(mimeType).First();
        }
    }
}
