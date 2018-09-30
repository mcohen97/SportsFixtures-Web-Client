using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    public class EntityAlreadyExistsException:Exception
    {
        public EntityAlreadyExistsException() {
        }
        public EntityAlreadyExistsException(string message):base(message) {

        }

        public EntityAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
