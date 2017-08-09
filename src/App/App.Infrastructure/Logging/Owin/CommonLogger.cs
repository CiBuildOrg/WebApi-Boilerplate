using System;
using System.Diagnostics;
using Common.Logging;
using Microsoft.Owin.Logging;

namespace App.Infrastructure.Logging.Owin
{
    public class CommonLogger : ILogger
    {
        protected readonly ILog MCommonLog;
        protected readonly Func<ILog, TraceEventType, bool> MIsLogEventEnabledFunc;
        protected readonly Action<ILog, TraceEventType, string, Exception> MWriteLogEventFunc;

        public CommonLogger(ILog commonLog, Func<ILog, TraceEventType, bool> isLogEventEnabledFunc, Action<ILog, TraceEventType, string, Exception> writeLogEventFunc)
        {
            MCommonLog = commonLog;
            MIsLogEventEnabledFunc = isLogEventEnabledFunc;
            MWriteLogEventFunc = writeLogEventFunc;
        }

        public bool WriteCore(TraceEventType traceEventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            if (state == null)
            {
                return MIsLogEventEnabledFunc(MCommonLog, traceEventType);
            }

            // no need to continue if event type isn't enabled
            if (!MIsLogEventEnabledFunc(MCommonLog, traceEventType))
            {
                return false;
            }

            MWriteLogEventFunc(MCommonLog, traceEventType, formatter(state, exception), exception);
            return true;
        }
    }
}
