using System;
using App.Entities.Contracts;

namespace App.Entities
{
    public class UserProfile : IBaseEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; }
        public string Description { get; set; }
    }
}