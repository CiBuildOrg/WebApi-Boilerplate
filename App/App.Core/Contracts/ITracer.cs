using System.Collections.Generic;
using App.Dto.Traces;

namespace App.Core.Contracts
{
    public interface ITracer
    {
        void WriteMessage(string source, string frame, string message);
        void WriteException(string source, string frame, string exception, string description, string name);

        void WriteOperation(string source, string frame,
            string description, string name, string operationMetadata);
        List<TraceStep> TraceSteps { get; }
        void Clear();
    }
}