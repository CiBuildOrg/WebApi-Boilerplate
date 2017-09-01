using System.IO;
using App.Dto.Response;

namespace App.Services.Contracts
{
    public interface IImageProcessorService
    {
        MemoryStream ProcessAvatar(byte[] image);
        ImageResponse GetDefaultImage();
    }
}