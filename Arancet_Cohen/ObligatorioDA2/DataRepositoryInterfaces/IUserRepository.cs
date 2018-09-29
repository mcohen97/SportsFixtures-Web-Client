using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using RepositoryInterface;

namespace DataRepositoryInterfaces
{
    public interface IUserRepository:IRepository<User,string>
    {
        User Get(User asked);

        void Delete(User toDelete);
    }
}
