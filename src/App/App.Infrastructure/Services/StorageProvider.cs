using System.IO;
using App.Infrastructure.Contracts;

namespace App.Infrastructure.Services
{
    public class StorageProvider : IStorageProvider
    {
        private const int BufferSize = 4096;

        public void StoreFile(string path, byte[] payload)
        {
            FileStream fileStream = null;

            var directoryToWriteTo = Path.GetDirectoryName(path);

            if (directoryToWriteTo != null && !Directory.Exists(directoryToWriteTo))
            {
                Directory.CreateDirectory(directoryToWriteTo);
            }

            try
            {
                fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None, BufferSize, true);
                fileStream.Write(payload, 0, payload.Length);
            }
            finally
            {
                fileStream?.Close();
            }
        }

        public byte[] GetFile(string path)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, true);
                var file = new byte[fileStream.Length];
                fileStream.Read(file, 0, (int)fileStream.Length);
                return file;
            }
            finally
            {
                fileStream?.Close();
            }
        }
    }
}