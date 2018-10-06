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

    }
}