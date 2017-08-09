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

    public interface ITraceTerminal
    {
        void AcceptMessage(TraceMessageInfo info);
        void AcceptException(TraceExceptionInfo info);
        void AcceptOperation(TraceOperationInfo info);

        List<TraceStep> TraceSteps { get; }
    }
}