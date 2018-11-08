using ObligatorioDA2.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Exceptions
{
    public class NoPermissionsException: ServiceException
    {
        public NoPermissionsException() : base("You don't have permissions for this action", ErrorType.NO_PERMISSION) { }
    }
}
