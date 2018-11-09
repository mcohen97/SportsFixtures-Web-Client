using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.Services.Exceptions
{
    [Serializable]
    public class WrongFixtureException : ServiceException
    {
        public WrongFixtureException(string message) : base(message, ErrorType.ENTITY_ALREADY_EXISTS)
        {
        }

    }
}