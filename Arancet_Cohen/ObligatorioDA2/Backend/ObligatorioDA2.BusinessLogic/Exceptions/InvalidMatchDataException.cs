namespace ObligatorioDA2.BusinessLogic.Exceptions
{
    [System.Serializable]
    public class InvalidMatchDataException : System.Exception
    {
        public InvalidMatchDataException() { }
        public InvalidMatchDataException(string message) : base(message) { }

    }
}