using System;

namespace BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidSportDataException : System.Exception
    {
        public InvalidSportDataException(string message) : base(message) { }
    }
    
}