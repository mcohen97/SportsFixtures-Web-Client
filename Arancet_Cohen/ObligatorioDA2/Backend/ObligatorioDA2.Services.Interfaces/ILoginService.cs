using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IAuthenticationService
    {
        User Login(string username, string password);
    }
}
