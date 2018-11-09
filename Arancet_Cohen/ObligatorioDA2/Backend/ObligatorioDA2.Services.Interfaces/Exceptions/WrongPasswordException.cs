using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Exceptions
{
    public class WrongPasswordException:ServiceException
    {
        public WrongPasswordException():base("Incorrect username or password", ErrorType.INVALID_DATA) {}
    }
}
