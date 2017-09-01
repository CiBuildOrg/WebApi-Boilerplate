using System;
using System.IO;
using App.Dto.Response;
using App.Infrastructure.Contracts;

namespace App.Infrastructure.Services
{
    public class ImageProcessorService : IImageProcessorService
    {
        private const string ImagePathTemplate = "~/images/{0}/{1}/{2}{3}";
        private const string DefaultUserSubDirectory = "users";

        public MemoryStream ProcessAvatar(byte[] image)
        {
            throw new NotImplementedException();
        }

        public ImageResponse GetDefaultImage()
        {
            throw new NotImplementedException();
        }
    }
}
