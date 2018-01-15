using System.Collections.Generic;

namespace App.Api.Security
{
    public class ProtectionConfig
    {
        /// <summary>
        /// Redirect url - user will be redirected here if they don't have the appropriate roles.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Allowed roles that can access the swagger documentation
        /// </summary>
        public List<string> AllowedRoles { get; set; }

        /// <summary>
        /// Swagger path
        /// </summary>
        public string ProtectPath { get; set; }
    }
}