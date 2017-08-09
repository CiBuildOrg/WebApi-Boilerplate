using System;
using System.Runtime.Serialization;

namespace App.Exceptions
{
    [Serializable]
    public class LockingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public LockingException()
        {
        }

        public LockingException(string messageFormat, params object[] parameters)
            : base(string.Format(messageFormat, parameters))
        {

        }

        public LockingException(string message)
            : base(message)
        {
        }

        public LockingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected LockingException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}