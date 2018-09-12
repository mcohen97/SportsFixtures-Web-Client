using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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
            User toReturn;
            using (DatabaseConnection db = creator.Get<DatabaseConnection>()) {
                    toReturn = db.Users.First(u => u.UserName.Equals(aUsername));              
            }
            return toReturn;
        }
    }
}
