using System.Collections.Generic;
using System.Linq;
using App.Core;
using App.Core.Contracts;
using App.Core.Utils;
using App.Dto.Request;
using App.Dto.Traces;
using App.Entities;
using App.Metrics.Concurrency;

namespace App.Infrastructure.Tracing
{
    public class SimpleTraceTerminal : ITraceTerminal
    {
        private readonly INow _now;
        private readonly IConfiguration _helper;
        private readonly object _thisLock = new object();
        private readonly ConcurrentList<TraceStep> _steps;
        private AtomicInteger _index;

        public SimpleTraceTerminal(INow now, IConfiguration helper)
        {
            _now = now;
            _helper = helper;
            using (new LockUtil(_thisLock))
            {
                _steps = new ConcurrentList<TraceStep>();
                _index = new AtomicInteger(0);
                _index.Increment();
            }
        }

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldTrace);

        public void AcceptMessage(TraceMessageInfo info)
        {
            if (ShouldLog)
            {
                using (new LockUtil(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Index = index,
                        Message = info.Message,
                        Frame = info.Frame,
                        Source = info.Source,
                        StepTimestamp = _now.UtcNow,
                        Metadata = string.Empty,
                        Type = StepType.Message,
                        Name = string.Empty
                    };

                    _steps.Add(step);

                    _index.Increment();
                }
            }
        }

        public void AcceptException(TraceExceptionInfo info)
        {
            if (ShouldLog)
            {
                using (new LockUtil(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Name = info.Name,
                        Index = index,
                        Frame = info.Frame,
                        Source = info.Source,
                        Metadata = info.Exception,
                        Type = StepType.Exception,
                        StepTimestamp = _now.UtcNow,
                        Message = info.Description
                    };

                    _steps.Add(step);

                    _index.Increment();
                }
            }
        }

        public void AcceptOperation(TraceOperationInfo info)
        {
            if (ShouldLog)
            {
                using (new LockUtil(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Index = index,
                        Frame = info.Frame,
                        Source = info.Source,
                        Metadata = info.Metadata,
                        Type = StepType.Operation,
                        StepTimestamp = _now.UtcNow,
                        Message = info.Description,
                        Name = info.Name
                    };

                    _steps.Add(step);

                    _index.Increment();
                }
            }
        }

        public List<TraceStep> TraceSteps
        {
            get
            {
                var steps = !ShouldLog ? new List<TraceStep>() : _steps.OrderBy(x => x.Index).ToList();
                return steps;
            }
        }
    }
}