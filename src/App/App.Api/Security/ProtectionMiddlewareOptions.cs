using System.Collections.Generic;

namespace App.Api.Security
{
    /// <summary>
    /// Swagger protect middleware options
    /// </summary>
    public class ProtectionMiddlewareOptions
    {
        public List<ProtectionConfig> Configs { get; set; }
    }
}