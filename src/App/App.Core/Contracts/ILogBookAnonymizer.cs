using App.Api.Enum;

namespace App.Core.Contracts
{
    public interface ILogBookAnonymizer
    {
        string Anonymize(string content, string path, Direction direction);
        object Anonymize(string input, string v, object @in);
    }
}