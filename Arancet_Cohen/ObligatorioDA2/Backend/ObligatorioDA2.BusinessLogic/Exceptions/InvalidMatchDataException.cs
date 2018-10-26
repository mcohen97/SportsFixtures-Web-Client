using System;

namespace ObligatorioDA2.BusinessLogic.Exceptions
{
    [System.Serializable]
    public class InvalidMatchDataException : InvalidDataException
    {
        public InvalidMatchDataException() { }
        public InvalidMatchDataException(string message) : base(message) { }

    }
}