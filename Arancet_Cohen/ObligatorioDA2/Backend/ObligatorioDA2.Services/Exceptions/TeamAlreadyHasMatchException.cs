using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.Services.Exceptions
{
    public class TeamAlreadyHasMatchException : Exception
    {
        public TeamAlreadyHasMatchException():base("One of these teams already plays in this date")
        {
        }

        public TeamAlreadyHasMatchException(string message) : base(message)
        {
        }
    }
}