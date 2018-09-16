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
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

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
            if (!AnyWithThisUserName(aUser.UserName))
            {
                AddNewUser(aUser);           
            }
            else {
                throw new UserAlreadyExistsException();
            }
        }

        private void AddNewUser(User aUser)
        {
            UserEntity toAdd = mapper.ToEntity(aUser);
            connection.Users.Add(toAdd);
            connection.SaveChanges();
        }

        public User GetUserByUsername(string aUserName)
        {
            User toReturn;
            if (AnyWithThisUserName(aUserName))
            {
                toReturn = GetExistentUser(aUserName);
            }
            else {
                throw new UserNotFoundException();
            }
            return toReturn;
        }

        private User GetExistentUser(string aUserName)
        {
            UserEntity fetched = connection.Users.First(u => u.UserName.Equals(aUserName));
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
            bool doesExist = AnyWithThisUserName(record.UserName);
            return doesExist;
        }

        private bool AnyWithThisUserName(string userName) {
           return connection.Users.Any(u => u.UserName.Equals(userName));
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
