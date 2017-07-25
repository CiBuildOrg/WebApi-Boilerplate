using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Dto.Traces;

namespace App.Services.Contracts
{
    public interface ILogService
    {
        void SaveTrace(ApiLogEntry entry, List<TraceStep> traceSteps);
    }
}
