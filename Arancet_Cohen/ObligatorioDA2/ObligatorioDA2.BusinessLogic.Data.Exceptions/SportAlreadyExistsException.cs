using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    [Serializable]
    public class SportAlreadyExistsException : Exception
    {
        public SportAlreadyExistsException()
        {
        }

        public SportAlreadyExistsException(string message) : base(message)
        {
        }

        public SportAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SportAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}