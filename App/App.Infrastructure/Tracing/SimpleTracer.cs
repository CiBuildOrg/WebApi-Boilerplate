using System;
using System.Collections.Generic;
using System.Linq;
using App.Core;
using App.Core.Contracts;
using App.Core.Utils;
using App.Dto.Traces;
using App.Entities;
using App.Metrics.Concurrency;

namespace App.Infrastructure.Tracing
{
    public class SimpleTracer : ITracer
    {
        private readonly INow _now;
        private readonly IConfiguration _helper;
        private readonly object _thisLock = new object();
        private ConcurrentList<TraceStep> _steps;
        private AtomicInteger _index;

        public SimpleTracer(INow now, IConfiguration helper)
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

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldLogSteps);

        public void WriteMessage(string source, string frame, string message)
        {
            if (ShouldLog)
            {
                using (new LockUtil(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Index = index,
                        Message = message,
                        Frame = frame,
                        Source = source,
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

        public void WriteException(string source, string frame, string exception, string description, string name)
        {
            if (ShouldLog)
            {
                using (new LockUtil(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Name = name,
                        Index = index,
                        Frame = frame,
                        Source = source,
                        Metadata = exception,
                        Type = StepType.Exception,
                        StepTimestamp = _now.UtcNow,
                        Message = description
                    };

                    _steps.Add(step);

                    _index.Increment();
                }
            }
        }

        public void WriteOperation(string source, string frame, string description, string name, string operationMetadata)
        {
            if (ShouldLog)
            {
                using (new LockUtil(_thisLock))
                {
                    var index = _index.GetValue();

                    var step = new TraceStep
                    {
                        Index = index,
                        Frame = frame,
                        Source = source,
                        Metadata = operationMetadata,
                        Type = StepType.Operation,
                        StepTimestamp = _now.UtcNow,
                        Message = description,
                        Name = name
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
            
        public void Clear()
        {
            using (new LockUtil(_thisLock))
            {
                _steps = new ConcurrentList<TraceStep>();
                _index = new AtomicInteger(0);
                _index.Increment();
            }
        }
    }
}