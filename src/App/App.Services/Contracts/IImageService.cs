using System;
using System.IO;
using App.Dto.Response;

namespace App.Services.Contracts
{
    public interface IImageService
    {
        void StoreImage(MemoryStream image, Guid imageId);
        ImageResponse GetImage(Guid imageId);
        void TryDelete(Guid imageId);
        ImageResponse GetDefaultImage();
    }
}