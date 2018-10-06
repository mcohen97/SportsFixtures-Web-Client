namespace BusinessLogic.Exceptions
{
    [System.Serializable]
    public class InvalidMatchDataExcpetion : System.Exception
    {
        public InvalidMatchDataExcpetion() { }
        public InvalidMatchDataExcpetion(string message) : base(message) { }

    }
}