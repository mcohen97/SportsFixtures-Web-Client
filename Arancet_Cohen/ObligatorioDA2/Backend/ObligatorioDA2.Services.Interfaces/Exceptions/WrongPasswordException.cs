using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Exceptions
{
    public class WrongPasswordException:Exception
    {
        public WrongPasswordException() {
        }

        public WrongPasswordException(string message) : base(message)
        {
        }
    }
}
