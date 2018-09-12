using System;
using System.Collections.Generic;
using System.Text;
using DataRepositoryInterfaces;
using DataAccess;
using BusinessLogic;

namespace DataRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ContextFactory creator;
        public UserRepository(ContextFactory aFactory)
        {
           creator = aFactory;
        }

        public User GetUserByUsername(string aUsername)
        {
            throw new NotImplementedException();
        }
    }
}
