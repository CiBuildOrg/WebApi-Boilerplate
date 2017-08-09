using System;
using App.Entities.Contracts;

namespace App.Entities
{
    public class Dummy : IBaseEntity
    {
        public Guid Id { get; set; }

        public string DummyData { get; set; }
    }
}
