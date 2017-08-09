using System;

namespace App.Core.Contracts
{
    public interface ITraceProvider : IDisposable
    {
        void WriteMessage(string message);
        void WriteException(Exception exception, string description);
        void WriteOperation(string description, string name, string operationMetadata);
    }
}   