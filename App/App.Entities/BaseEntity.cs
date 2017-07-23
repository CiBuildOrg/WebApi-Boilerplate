using System;
using App.Entities.Contracts;

namespace App.Entities
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
    }
}
