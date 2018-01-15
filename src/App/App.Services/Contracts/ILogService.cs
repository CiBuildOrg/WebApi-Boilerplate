using System.Collections.Generic;
using App.Dto.Request;
using App.Dto.Response;

namespace App.Services.Contracts
{
    public interface ILogService
    {
        List<TraceViewModel> GetTraces(TraceSearchRequest request);
    }
}
