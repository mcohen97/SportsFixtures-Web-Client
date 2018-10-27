using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    public class LogAlreadyExistsException : EntryPointNotFoundException
    {
        public LogAlreadyExistsException()
        {
        }

        public LogAlreadyExistsException(string message) : base("Log already exists")
        {
        }

        public LogAlreadyExistsException(string message, Exception inner) : base("Log already exists", inner)
        {
        }

        protected LogAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
