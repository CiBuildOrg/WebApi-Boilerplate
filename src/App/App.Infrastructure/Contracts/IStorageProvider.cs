using System.IO;

namespace App.Infrastructure.Contracts
{
    public interface IStorageProvider
    {
        void StoreFile(string path, byte[] payload);
        void StoreFile(string path, Stream payload);

        byte[] GetFile(string path);
    }
}