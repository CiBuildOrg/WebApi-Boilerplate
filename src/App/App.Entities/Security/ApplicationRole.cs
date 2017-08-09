using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App.Entities.Security
{
    public class ApplicationRole : IdentityRole<Guid, ApplicationIdentityUserRole>
    {
        public ApplicationRole() { }
        public ApplicationRole(string name)
        {
            Name = name;
        }

        public string RoleDescription { get; set; }
    }
}