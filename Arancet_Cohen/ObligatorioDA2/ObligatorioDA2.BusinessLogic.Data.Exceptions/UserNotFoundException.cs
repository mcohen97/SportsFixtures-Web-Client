using System;
using System.Runtime.Serialization;

namespace ObligatorioDA2.BusinessLogic.Data.Exceptions
{


    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}