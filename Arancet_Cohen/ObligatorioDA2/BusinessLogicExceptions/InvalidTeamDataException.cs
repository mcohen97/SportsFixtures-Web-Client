namespace BusinessLogicExceptions
{
    [System.Serializable]
    public class InvalidTeamDataException : System.Exception
    {
        public InvalidTeamDataException() { }
        public InvalidTeamDataException(string message) : base(message) { }
        public InvalidTeamDataException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidTeamDataException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}