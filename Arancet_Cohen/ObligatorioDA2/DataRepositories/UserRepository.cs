using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataRepositoryInterfaces;
using DataAccess;
using BusinessLogic;
using RepositoryInterface;

namespace DataRepositories
{
    public class UserRepository : IUserRepository, IRepository<User>
    {
        private readonly ContextFactory creator;
        public UserRepository(ContextFactory aFactory)
        {
           creator = aFactory;
        }

        public void Add(User entity)
        {
            using (DatabaseConnection dbConn = creator.Get()) {
                dbConn.Users.Add(entity);
                dbConn.SaveChanges();
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public bool Exists(User record)
        {
            throw new NotImplementedException();
        }

        public User Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public User Get(User asked)
        {
            throw new NotImplementedException();
        }

        public ICollection<User> GetAll()
        {
            ICollection<User> query;
            using (DatabaseConnection dbConn = creator.Get()) {
                query = dbConn.Users.ToList();
            }
            return query;
        }

        public User GetUserByUsername(string aUsername)
        {
            User toReturn;
            using (DatabaseConnection db = creator.Get()) {
                    toReturn = db.Users.First(u => u.UserName.Equals(aUsername));              
            }
            return toReturn;
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public void Modify(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
