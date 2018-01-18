using System.Collections.Generic;
using App.Dto.Request;
using App.Dto.Response;
using System;

namespace App.Services.Contracts
{
    public interface ILogService
    {
        Tuple<int, List<TraceViewModel>> GetTraces(TraceSearchRequest request);
    }
}
