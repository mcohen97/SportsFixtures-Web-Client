namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{
    [System.Serializable]
    public class TeamNotFoundException : System.Exception
    {
        public TeamNotFoundException() { }
        public TeamNotFoundException(string message) : base(message) { }
        public TeamNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected TeamNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}