using System;
using System.Runtime.Serialization;

namespace App.Exceptions
{
    [Serializable]
    public class NotAuthenticatedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NotAuthenticatedException()
        {
        }

        public NotAuthenticatedException(string message) : base(message)
        {
        }

        public NotAuthenticatedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NotAuthenticatedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}