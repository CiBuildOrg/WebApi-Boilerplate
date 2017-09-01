using System.IO;
using App.Dto.Response;

namespace App.Infrastructure.Contracts
{
    public interface IImageProcessorService
    {
        MemoryStream ProcessAvatar(byte[] image);
        ImageResponse GetDefaultImage();
    }
}