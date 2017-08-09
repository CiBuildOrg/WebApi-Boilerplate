using System;
using App.Entities;

namespace App.Dto.Traces
{
    public class TraceStep
    {
        public int Index { get; set; }

        public DateTime StepTimestamp { get; set; }

        public StepType Type { get; set; }

        public string Source { get; set; }

        public string Name { get; set; }

        public string Frame { get; set; }

        public string Message { get; set; }

        public string Metadata { get; set; }
    }
}
