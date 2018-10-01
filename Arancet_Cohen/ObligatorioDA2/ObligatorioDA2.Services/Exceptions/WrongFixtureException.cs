using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.Services.Exceptions
{
    [Serializable]
    public class WrongFixtureException : Exception
    {
        public WrongFixtureException()
        {
        }

        public WrongFixtureException(string message) : base(message)
        {
        }

        public WrongFixtureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongFixtureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}