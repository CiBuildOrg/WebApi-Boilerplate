using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Entities.Security
{
    public class CustomRole : IdentityRole<Guid, IdentityUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name)
        {
            Name = name;
        }
    }
}