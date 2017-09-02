using Newtonsoft.Json;

namespace App.Api.ErrorHandling
{
    public class ExceptionResponse
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "dump")]
        public string Dump { get; set; }
    }
}