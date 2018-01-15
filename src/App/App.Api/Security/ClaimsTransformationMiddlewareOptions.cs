using System;
using System.Security.Claims;
using App.Database;
using Microsoft.Owin;

namespace App.Api.Security
{
    /// <summary>
    /// claims transform options
    /// </summary>
    public class ClaimsTransformationMiddlewareOptions
    {
        /// <summary>
        /// Claims Principal factory
        /// </summary>
        public Func<IOwinContext, ClaimsPrincipal> GetClaim { get; set; }

        /// <summary>
        /// Database context
        /// </summary>
        public DatabaseContext Context { get; set; }

        /// <summary>
        /// Claim name identifier
        /// </summary>
        public string NameIdentifier { get; set; }
    }
}