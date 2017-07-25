using System;
using System.Diagnostics;
using App.Core;
using App.Core.Contracts;
using ProductionStackTrace;

namespace App.Infrastructure.Tracing
{
    public class TraceStepper : ITraceStepper
    {
        private readonly ITracer _tracer;
        private readonly IConfiguration _helper;

        public TraceStepper(ITracer tracer, IConfiguration helper)
        {
            _tracer = tracer;
            _helper = helper;
        }

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldLogSteps);

        public void WriteMessage(string message)
        {
            if (ShouldLog)
            {
                var stackTrace = new StackTrace();

                var stackTraceInfo = StackTraceParser.Parse(
                    stackTrace.ToString(),
                    (f, t, m, pl, ps, fn, ln) => new
                    {
                        Frame = f,
                        Type = t,
                        Method = m,
                        ParameterList = pl,
                        Parameters = ps,
                        File = fn,
                        Line = ln,
                    });


                string methodName = null;
                string frame = null;

                var @break = false;
                foreach (var stackTraceEntry in stackTraceInfo)
                {
                    if (@break)
                    {
                        methodName = stackTraceEntry.Method;
                        frame = stackTraceEntry.Frame;

                        break;
                    }

                    var type = stackTraceEntry.Type;
                    if (string.IsNullOrEmpty(type)) continue;

                    var @typeof = typeof(TraceStepper).Name;
                    if (type.EndsWith(@typeof))
                    {
                        @break = true;
                    }
                }

                _tracer.WriteMessage(new TraceMessageInfo
                {
                    Frame = frame,
                    Source = methodName,
                    Message = message
                });
            }
        }

        public void WriteException(Exception exception, string description)
        {
            if (ShouldLog)
            {
                var stackTrace = new StackTrace();

                var stackTraceInfo = StackTraceParser.Parse(
                    stackTrace.ToString(),
                    (f, t, m, pl, ps, fn, ln) => new
                    {
                        Frame = f,
                        Type = t,
                        Method = m,
                        ParameterList = pl,
                        Parameters = ps,
                        File = fn,
                        Line = ln,
                    });
                string methodName = null;
                string frame = null;

                var @break = false;
                foreach (var stackTraceEntry in stackTraceInfo)
                {
                    if (@break)
                    {
                        methodName = stackTraceEntry.Method;
                        frame = stackTraceEntry.Frame;

                        break;
                    }

                    var type = stackTraceEntry.Type;
                    if (string.IsNullOrEmpty(type)) continue;

                    var @typeof = typeof(TraceStepper).Name;
                    if (type.EndsWith(@typeof))
                    {
                        @break = true;
                    }
                }

                var exceptionName = exception.GetType().Name;
                var exceptionTrace = ExceptionReporting.GetExceptionReport(exception);
                _tracer.WriteException(new TraceExceptionInfo
                {
                    Frame = frame,
                    Exception = exceptionTrace,
                    Source = methodName,
                    Description = description,
                    Name = exceptionName
                });
            }
        }

        public void WriteOperation(string description, string name, string operationMetadata)
        {
            if (!ShouldLog) return;
            var stackTrace = new StackTrace();

            var stackTraceInfo = StackTraceParser.Parse(
                stackTrace.ToString(),
                (f, t, m, pl, ps, fn, ln) => new
                {
                    Frame = f,
                    Type = t,
                    Method = m,
                    ParameterList = pl,
                    Parameters = ps,
                    File = fn,
                    Line = ln,
                });

            string methodName = null;
            string frame = null;

            var @break = false;
            foreach (var stackTraceEntry in stackTraceInfo)
            {
                if (@break)
                {
                    methodName = stackTraceEntry.Method;
                    frame = stackTraceEntry.Frame;

                    break;
                }

                var type = stackTraceEntry.Type;
                if (string.IsNullOrEmpty(type)) continue;

                var @typeof = typeof(TraceStepper).Name;
                if (type.EndsWith(@typeof))
                {
                    @break = true;
                }
            }

            _tracer.WriteOperation(new TraceOperationInfo
            {
                Metadata = operationMetadata,
                Frame = frame,
                Description = description,
                Name = name,
                Source = methodName
            });
        }

        public void Dispose()
        {
        }
    }
}
