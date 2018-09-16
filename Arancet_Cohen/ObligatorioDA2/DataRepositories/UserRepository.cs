using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataRepositoryInterfaces;
using DataAccess;
using BusinessLogic;
using RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.DataAccess.Domain.Mappers;

namespace DataRepositories
{
    public class UserRepository : IUserRepository, IRepository<User>
    {
        private readonly DatabaseConnection connection;
        private readonly UserMapper mapper;

        public UserRepository(DatabaseConnection aConnection)
        {
           connection = aConnection;
           mapper = new UserMapper();
        }

        public void Add(User aUser) {
            UserEntity toAdd = mapper.ToEntity(aUser);
            connection.Users.Add(toAdd);
            connection.SaveChanges();
        }
        public User GetUserByUsername(string aUsername)
        {
            UserEntity fetched = connection.Users.First(u => u.UserName.Equals(aUsername));
            User toReturn = new Admin(fetched.Name, fetched.Surname, fetched.UserName, fetched.Password, fetched.Email);
            return toReturn;
        }

        public ICollection<User> GetAll(){
            IQueryable<User> query = connection.Users.Select(u => mapper.ToUser(u));
            return query.ToList();
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
