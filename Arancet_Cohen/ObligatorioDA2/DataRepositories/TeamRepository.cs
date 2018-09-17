using System;
using System.Collections.Generic;
using BusinessLogic;
using DataRepositoryInterfaces;
using RepositoryInterface;

namespace DataRepositories
{
    public class TeamRepository : IUserRepository, IRepository<User>
    {
        public void Add(User entity)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string aUsername)
        {
            throw new System.NotImplementedException();
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