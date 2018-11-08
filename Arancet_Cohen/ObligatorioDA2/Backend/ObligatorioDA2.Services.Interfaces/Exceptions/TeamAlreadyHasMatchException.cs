using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.Services.Exceptions
{
    public class TeamAlreadyHasMatchException : ServiceException
    {
        public TeamAlreadyHasMatchException():base("One of these teams already plays in this date", ErrorType.ENTITY_ALREADY_EXISTS)
        {
        }

        public TeamAlreadyHasMatchException(string message) : base(message, ErrorType.ENTITY_ALREADY_EXISTS)
        {
        }
    }
}