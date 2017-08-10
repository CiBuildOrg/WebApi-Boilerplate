using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Security
{
    public class OwinEnvironment
    {
        /// <summary>
        /// Gets OWIN property name of allowedOrigin
        /// </summary>
        public const string ClientAllowedOriginPropertyName = "as:clientAllowedOrigin";

        /// <summary>
        /// Gets OWIN property name of refresh token life time
        /// </summary>
        public const string ClientRefreshTokenLifeTimePropertyName = "as:clientRefreshTokenLifeTime";

        /// <summary>
        /// Gets OWIN property name of audience (client id)
        /// </summary>
        public const string ClientPropertyName = "as:client_id";

        public const string UserPropertyName = "as:user_id";
    }
}
