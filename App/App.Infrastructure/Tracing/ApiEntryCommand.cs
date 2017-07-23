using System.Collections.Generic;
using App.Dto.Traces;
using Enexure.MicroBus;

namespace App.Infrastructure.Tracing
{
    public class ApiEntryCommand : ICommand
    {
        public ApiLogEntry Entry { get; }
        public List<TraceStep> Steps { get; }

        public ApiEntryCommand(ApiLogEntry entry, List<TraceStep> steps)
        {
            Entry = entry;
            Steps = steps;
        }
    }
}