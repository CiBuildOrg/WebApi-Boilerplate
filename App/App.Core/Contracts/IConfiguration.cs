namespace App.Core.Contracts
{
    public interface IConfiguration
    {
        string GetString(string configurationKey);
        int GetInt(string configurationKey);
        bool GetBool(string configurationKey);
    }
}