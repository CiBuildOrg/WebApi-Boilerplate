using System;
using System.Collections.Generic;
using App.Entities.Contracts;

namespace App.Entities
{
    public class LogEntry : IBaseEntity
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime? RequestTimestamp { get; set; }

        public DateTime? ResponseTimestamp { get; set; }

        public string RequestUri { get; set; }

        public virtual ICollection<LogStep> Steps { get; set; }
    }
}