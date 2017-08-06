using System;
using App.Entities.Contracts;

namespace App.Entities.Security
{
    public class Client : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Secret { get; set; }

        public string Name { get; set; }

        public ApplicationType ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }

        public string AllowedOrigin { get; set; }
    }
}
