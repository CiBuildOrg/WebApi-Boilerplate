using System.Collections.Generic;
using App.Dto.Request;
using App.Dto.Response;
using App.Dto.Traces;

namespace App.Services.Contracts
{
    public interface ILogService
    {
        void SaveTrace(ApiLogEntry entry, List<TraceStep> traceSteps);
        List<TraceViewModel> GetTraces(TraceSearchRequest request);
    }
}
