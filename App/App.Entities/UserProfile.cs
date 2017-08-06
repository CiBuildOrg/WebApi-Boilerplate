using System;
using App.Entities.Contracts;

namespace App.Entities
{
    public class UserProfile : IBaseEntity
    {
        public Guid Id { get; set; }

        public string AboutMe { get; set; }

        public string PhoneNumber { get; set; }
    }
}