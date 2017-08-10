using System.Collections.Generic;
using App.Dto.Request;
using App.Dto.Traces;

namespace App.Core.Contracts
{
    

    public interface ITraceTerminal
    {
        void AcceptMessage(TraceMessageInfo info);
        void AcceptException(TraceExceptionInfo info);
        void AcceptOperation(TraceOperationInfo info);

        List<TraceStep> TraceSteps { get; }
    }
}