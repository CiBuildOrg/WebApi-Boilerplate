using System;
using System.Collections.Generic;
using System.Linq;
using App.Core;
using App.Core.Contracts;
using App.Dto.Traces;
using App.Entities;
using App.Services.Contracts;

namespace App.Services
{
    public class LogService : ILogService
    {
        private readonly IDatabaseContext _context;
        private readonly INow _now;
        private readonly IConfiguration _helper;

        public LogService(IDatabaseContext context, INow now, IConfiguration helper)
        {
            _context = context;
            _now = now;
            _helper = helper;
        }

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldTrace);

        public void SaveTrace(ApiLogEntry entry, List<TraceStep> traceSteps)
        {
            if (!ShouldLog) return;

            var logEntry = new LogEntry
            {
                Id = Guid.NewGuid(),
                Timestamp = _now.UtcNow,
                RequestTimestamp = entry.RequestTimestamp,
                ResponseTimestamp = entry.ResponseTimestamp,
                RequestUri = entry.RequestUri,
                Steps = new List<LogStep>()
            };

            foreach (var logStep in traceSteps.OrderBy(x => x.Index).ToList().Select(step => new LogStep
            {
                Id = Guid.NewGuid(),
                LogEntry = logEntry,
                LogEntryId = logEntry.Id,
                Index = step.Index,
                Metadata = step.Metadata,
                StepTimestamp = step.StepTimestamp,
                Type = step.Type,
                Frame = step.Frame,
                Name = step.Name,
                Message = step.Message,
                Source = step.Source,
            }))
            {
                logEntry.Steps.Add(logStep);
            }

            _context.Save(logEntry);
        }
    }
}
