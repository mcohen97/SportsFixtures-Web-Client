namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    [System.Serializable]
    public class TeamAlreadyExistsException : System.Exception
    {
        public TeamAlreadyExistsException() { }
        public TeamAlreadyExistsException(string message) : base(message) { }
        public TeamAlreadyExistsException(string message, System.Exception inner) : base(message, inner) { }
        protected TeamAlreadyExistsException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}