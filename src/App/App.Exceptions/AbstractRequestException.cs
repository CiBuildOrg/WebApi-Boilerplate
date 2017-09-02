using System;

namespace App.Exceptions
{
    public abstract class AbstractRequestException : Exception
    {
        public int Code { get; }
        public string ExceptionMessage { get; }

        public string DataDump { get; }

        protected AbstractRequestException(int code, string message)
        {
            Code = code;
            ExceptionMessage = message;
        }

        protected AbstractRequestException(int code, string message, string dataDump)
        {
            Code = code;
            ExceptionMessage = message;
            DataDump = dataDump;
        }
    }
}