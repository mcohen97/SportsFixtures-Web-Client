using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;

namespace DataRepositoryInterfaces
{
    public interface IUserRepository
    {
        User GetUserByUsername(string aUsername);
        
    }
}
