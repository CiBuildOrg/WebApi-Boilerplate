using System;
using System.IO;

namespace App.Services.Contracts
{
    public interface IImageService
    {
        void StoreImage(MemoryStream image, Guid imageId);
        byte[] GetImage(Guid imageId);
    }
}