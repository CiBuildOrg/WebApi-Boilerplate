namespace App.Infrastructure.Contracts
{
    public interface IStorageProvider
    {
        void StoreFile(string path, byte[] payload);
        byte[] GetFile(string path);
    }
}