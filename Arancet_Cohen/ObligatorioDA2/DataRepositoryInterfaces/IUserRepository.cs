using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using RepositoryInterface;

namespace DataRepositoryInterfaces
{
    public interface IUserRepository:IRepository<User>
    {
        User GetUserByUsername(string aUsername);

        User Get(User asked);

        void Delete(User toDelete);
    }
}
