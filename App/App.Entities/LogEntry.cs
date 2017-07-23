using System;
using System.Collections.Generic;

namespace App.Entities
{
    public class LogEntry : BaseEntity
    {
        public DateTime Timestamp { get; set; }

        public DateTime? RequestTimestamp { get; set; }

        public DateTime? ResponseTimestamp { get; set; }

        public string RequestUri { get; set; }

        public virtual ICollection<LogStep> Steps { get; set; }
    }
}