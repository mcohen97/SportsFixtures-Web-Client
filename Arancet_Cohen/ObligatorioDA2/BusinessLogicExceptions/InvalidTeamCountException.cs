namespace BusinessLogic.Exceptions
{
    public class InvalidTeamCountException : System.Exception
    {
        public InvalidTeamCountException() { }
        public InvalidTeamCountException(string message) : base(message) { }
        public InvalidTeamCountException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidTeamCountException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}