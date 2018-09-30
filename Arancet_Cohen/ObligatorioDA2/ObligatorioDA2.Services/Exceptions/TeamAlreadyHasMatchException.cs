using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.Services.Exceptions
{
    public class TeamAlreadyHasMatchException : Exception
    {
        public TeamAlreadyHasMatchException()
        {
        }

        public TeamAlreadyHasMatchException(string message) : base(message)
        {
        }

        public TeamAlreadyHasMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TeamAlreadyHasMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}