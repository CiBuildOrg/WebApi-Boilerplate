using System;
using System.Security.Claims;
using System.Security.Principal;
using App.Entities;

namespace App.Infrastructure.Security
{
    public class AppClaimsPrincipal : ClaimsPrincipal
    {
        public UserProfile UserProfile { get; }

        public AppClaimsPrincipal(IPrincipal principal, UserProfile userProfile) : base(principal)
        {
            UserProfile = userProfile;
        }
    }
}