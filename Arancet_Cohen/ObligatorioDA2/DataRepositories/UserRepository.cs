using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataRepositoryInterfaces;
using DataAccess;
using BusinessLogic;
using RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace DataRepositories
{
    public class UserRepository : IUserRepository, IRepository<User>
    {
        private readonly DatabaseConnection connection;

        public UserRepository(DatabaseConnection aConnection)
        {
           connection = aConnection;
        }

        public void Add(User aUser){

        }
        public User GetUserByUsername(string aUsername)
        {
            return null;
        }

        public ICollection<User> GetAll(){
            return null;
        }

        public bool IsEmpty()
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

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Modify(User entity)
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
    }
}
