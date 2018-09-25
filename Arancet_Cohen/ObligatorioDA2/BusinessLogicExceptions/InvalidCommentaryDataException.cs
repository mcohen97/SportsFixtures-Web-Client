namespace BusinessLogic.Exceptions
{
    [System.Serializable]
    public class InvalidCommentaryDataException : System.Exception
    {
        public InvalidCommentaryDataException() { }
        public InvalidCommentaryDataException(string message) : base(message) { }
        public InvalidCommentaryDataException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidCommentaryDataException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}