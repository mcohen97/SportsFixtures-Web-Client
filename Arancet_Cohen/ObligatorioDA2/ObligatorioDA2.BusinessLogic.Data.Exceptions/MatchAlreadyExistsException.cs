using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    public class MatchAlreadyExistsException : EntityAlreadyExistsException
    {
        public MatchAlreadyExistsException()
        {
        }

        public MatchAlreadyExistsException(string message) : base(message)
        {
        }

        public MatchAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MatchAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
