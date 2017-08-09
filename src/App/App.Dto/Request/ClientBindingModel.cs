using System.ComponentModel.DataAnnotations;
using App.Entities.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace App.Dto.Request
{
    public class 
        ClientBindingModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ApplicationType ApplicationType { get; set; }

        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }

        [MaxLength(200)]
        public string AllowedOrigin { get; set; }
    }
}