using System.Diagnostics.CodeAnalysis;
using System.IO;
using App.Services.Contracts;

namespace App.Services
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class StorageProvider : IStorageProvider
    {
        private const int BufferSize = 4096;

        public void StoreFile(string path, Stream payload)
        {
            var array = ReadFully(payload);
            StoreFile(path, array);
        }

        public void StoreFile(string path, byte[] payload)
        {
            FileStream fileStream = null;

            var directoryToWriteTo = Path.GetDirectoryName(path);

            if (directoryToWriteTo != null && !Directory.Exists(directoryToWriteTo))
            {
                Directory.CreateDirectory(directoryToWriteTo);
            }

            if(File.Exists(path))
                File.Delete(path);

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

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}