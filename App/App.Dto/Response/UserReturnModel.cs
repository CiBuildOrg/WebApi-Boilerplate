using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace App.Dto.Response
{
    public class UserReturnModel
    {
        public string Url { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Description { get; set; }
        public DateTime JoinDate { get; set; }
        public IList<string> Roles { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}