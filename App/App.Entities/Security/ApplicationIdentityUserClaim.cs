using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Entities.Security
{
    public class ApplicationIdentityUserClaim : IdentityUserClaim<Guid>
    {
    }
}