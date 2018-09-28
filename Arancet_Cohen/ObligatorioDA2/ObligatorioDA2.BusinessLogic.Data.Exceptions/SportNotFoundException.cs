using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    [Serializable]
    public class SportNotFoundException : Exception
    {
        public SportNotFoundException()
        {
        }

        public SportNotFoundException(string message) : base(message)
        {
        }

        public SportNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SportNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}