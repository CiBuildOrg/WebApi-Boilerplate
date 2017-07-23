using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core;
using App.Core.Contracts;
using App.Entities;
using Enexure.MicroBus;

namespace App.Infrastructure.Tracing
{
    public class ApiEntryHandler : ICommandHandler<ApiEntryCommand>
    {
        private readonly IDatabaseContext _context;
        private readonly INow _now;
        private readonly IConfiguration _helper;

        public ApiEntryHandler(IDatabaseContext context, INow now, IConfiguration helper)
        {
            _context = context;
            _now = now;
            _helper = helper;
        }

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldLogSteps);

        public Task Handle(ApiEntryCommand command)
        {
            if (ShouldLog)
            {
                var logEntry = new LogEntry
                {
                    Id = Guid.NewGuid(),
                    Timestamp = _now.UtcNow,
                    RequestTimestamp = command.Entry.RequestTimestamp,
                    ResponseTimestamp = command.Entry.ResponseTimestamp,
                    RequestUri = command.Entry.RequestUri,
                    Steps = new List<LogStep>()
                };

                foreach (var logStep in command.Steps.OrderBy(x => x.Index).ToList().Select(step => new LogStep
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

            return Task.FromResult(0);
        }
    }
}