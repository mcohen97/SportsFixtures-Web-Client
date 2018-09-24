namespace BusinessLogic.Exceptions
{
    [System.Serializable]
    public class InvalidMatchDataExcpetion : System.Exception
    {
        public InvalidMatchDataExcpetion() { }
        public InvalidMatchDataExcpetion(string message) : base(message) { }
        public InvalidMatchDataExcpetion(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidMatchDataExcpetion(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}