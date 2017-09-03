using System;

namespace App.Exceptions
{
    [Serializable]
    public class ContextException : AbstractRequestException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ContextException(string message)
            : base(100, message)
        {
        }

        public ContextException(string message, Exception inner)
            : base(100, message, inner.Message)
        {
        }
    }
}