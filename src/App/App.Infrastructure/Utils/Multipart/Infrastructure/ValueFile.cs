using App.Dto.Request;

namespace App.Infrastructure.Utils.Multipart.Infrastructure
{
    public class ValueFile
    {
        public string Name { get; set; }
        public HttpFile Value { get; set; }
    }
}