using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositoryInterfaces
{
    public interface ILoginService
    {
        User Login(string username, string password);
    }
}
