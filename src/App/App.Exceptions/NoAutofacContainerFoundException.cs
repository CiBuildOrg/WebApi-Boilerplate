using System;
using System.Runtime.Serialization;

namespace App.Exceptions
{
    [Serializable]
    public class NoAutofacContainerFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NoAutofacContainerFoundException()
        {
        }

        public NoAutofacContainerFoundException(string message) : base(message)
        {
        }

        public NoAutofacContainerFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NoAutofacContainerFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}