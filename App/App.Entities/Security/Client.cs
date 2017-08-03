using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Security
{
    public class Client
    {
        public Guid Id { get; set; }

        public string Secret { get; set; }

        public string Name { get; set; }

        public ApplicationType ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }

        public string AllowedOrigin { get; set; }
    }

    public enum ApplicationType
    {
        JavaScript = 0,
        NativeConfidential = 1,
        ExternalInterface = 2,
    }
}
