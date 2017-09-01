﻿using System;
using System.IO;
using System.Linq;
using System.Web;
using App.Dto.Response;
using App.Entities;
using App.Infrastructure.Contracts;

namespace App.Infrastructure.Services
{
    public class ImageProcessorService : IImageProcessorService
    {
        private readonly IStorageProvider _storageProvider;
        private const string ImagePathTemplate = "~/images/{0}/{1}/{2}{3}";
        private const string DefaultUserSubDirectory = "users";

        private const string DefaultMimeType = "image/jpeg";

        public ImageProcessorService(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public MemoryStream ProcessAvatar(byte[] image)
        {
            throw new NotImplementedException();
        }

        public ImageResponse GetDefaultImage()
        {


            var defaultImagePath =
                HttpContext.Current.Server.MapPath(string.Format(ImagePathTemplate,
                    DefaultUserSubDirectory,
                    ReturnSizeSubdirectory(ImageSize.Small),
                    "default",
                    GetImageExtension(DefaultMimeType)));

            if (!File.Exists(defaultImagePath))
            {
                throw new FileNotFoundException("Default avatar image not found");
            }

            // load the image from the file storage ( implemented as a naive directory-based store )
            var imagePayload = _storageProvider.GetFile(defaultImagePath);

            return new ImageResponse
            {
                MimeType = DefaultMimeType,
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
