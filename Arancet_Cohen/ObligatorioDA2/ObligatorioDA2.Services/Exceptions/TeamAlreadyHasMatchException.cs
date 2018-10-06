using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.Services.Exceptions
{
    public class TeamAlreadyHasMatchException : Exception
    {
        public TeamAlreadyHasMatchException()
        {
        }

        public TeamAlreadyHasMatchException(string message) : base(message)
        {
        }
    }
}