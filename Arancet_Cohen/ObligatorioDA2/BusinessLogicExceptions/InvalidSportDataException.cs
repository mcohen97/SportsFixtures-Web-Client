using System;

namespace BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidSportDataException : System.Exception
    {
        public InvalidSportDataException() { }
        public InvalidSportDataException(string message) : base(message) { }
        public InvalidSportDataException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidSportDataException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
}