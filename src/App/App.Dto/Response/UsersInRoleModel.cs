using System;
using System.Collections.Generic;

namespace App.Dto.Response
{
    public class UsersInRoleModel
    {

        public Guid Id { get; set; }
        public List<Guid> EnrolledUsers { get; set; }
        public List<Guid> RemovedUsers { get; set; }
    }
}