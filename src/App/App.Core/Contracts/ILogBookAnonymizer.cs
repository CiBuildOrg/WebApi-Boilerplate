using App.Api.Enum;

namespace App.Core.Contracts
{
    public interface ILogBookAnonymizer
    {
        string Anonymize(string content, string path, Direction direction);
    }
}