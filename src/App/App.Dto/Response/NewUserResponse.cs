using Newtonsoft.Json;

namespace App.Dto.Response
{
    public class NewUserResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}