using System;

namespace App.Entities
{
    public class LogStep : BaseEntity
    {
        public int Index { get; set; }
        public DateTime StepTimestamp { get; set; }
        public StepType Type { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }
        public string Frame { get; set; }
        public string Metadata { get; set; }
        public string Message { get; set; }
        public virtual LogEntry LogEntry { get; set; }
        public Guid LogEntryId { get; set; }
    }
}