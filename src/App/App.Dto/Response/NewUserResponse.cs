using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Dto.Response
{
    public class NewUserResponse
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public IEnumerable<Error> Errors { get; set; }

        public static NewUserResponse Ok => new NewUserResponse {Success = true};
    }

    public class Error
    {
        [JsonProperty(PropertyName = "error")]
        public string ErrorMessage { get; set; }
    }
}