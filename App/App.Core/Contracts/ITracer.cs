using System;
using System.Collections.Generic;
using App.Dto.Traces;

namespace App.Core.Contracts
{
    public class TraceMessageBase
    {
        public string Source { get; set; }
        public string Frame { get; set; }
    }

    public class TraceMessageInfo : TraceMessageBase
    {
        public string Message { get; set; }
    }

    public class TraceExceptionInfo : TraceMessageBase
    {
        public string Exception { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class TraceOperationInfo : TraceMessageBase
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Metadata { get; set; }
    }

    public interface ITracer
    {
        void WriteMessage(TraceMessageInfo info);
        void WriteException(TraceExceptionInfo info);
        void WriteOperation(TraceOperationInfo info);

        List<TraceStep> TraceSteps { get; }
    }
}