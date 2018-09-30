using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IUserService
    {
        User GetUser(string username);
    }
}
