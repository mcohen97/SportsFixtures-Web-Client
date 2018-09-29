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
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnection context;
        private readonly UserMapper mapper;

        public UserRepository(DatabaseConnection aContext)
        {
            context = aContext;
            mapper = new UserMapper();
        }

        public void Add(User aUser)
        {
            UserEntity entity = mapper.ToEntity(aUser);
            if (!AnyWithThisUserName(aUser.UserName))
            {
                context.Users.Add(entity);
                context.SaveChanges();
            }
            else
            {
                throw new UserAlreadyExistsException();
            }
        }

        public User Get(string aUserName)
        {
            User toReturn;
            if (AnyWithThisUserName(aUserName))
            {
                UserEntity retrieved = GetEntityByUsername(aUserName);
                toReturn = mapper.ToUser(retrieved);
            }
            else
            {
                throw new UserNotFoundException();
            }
            return toReturn;
        }

        public void Delete(string username)
        {
            if (AnyWithThisUserName(username))
            {
                UserEntity toDelete = GetEntityByUsername(username);
                context.Users.Remove(toDelete);
                context.SaveChanges();
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        private UserEntity GetEntityByUsername(string aUserName) {
            return context.Users.First(u => u.UserName.Equals(aUserName));
        }

        public ICollection<User> GetAll()
        {
            IQueryable<UserEntity> query = context.Users;
            ICollection<User> users = query.Select(u => mapper.ToUser(u)).ToList();
            return users;
        }

        public bool IsEmpty()
        {
            return !context.Users.Any();
        }

       

        public bool Exists(User entity)
        {
            UserEntity record = mapper.ToEntity(entity);
            bool doesExist = context.Users.Any(u => u.UserName.Equals(entity.UserName));
            return doesExist;
        }

        public void Modify(User aUser)
        {
            if (AnyWithThisUserName(aUser.UserName))
            {
                UserEntity entity = mapper.ToEntity(aUser);
                context.Update(entity);
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        private bool AnyWithThisUserName(string userName)
        {
            return context.Users.Any(u => u.UserName.Equals(userName));
        }

        public User Get(User asked)
        {
            return Get(asked.UserName);
        }


        public void Clear()
        {
            IQueryable<UserEntity> allOfThem = context.Users;
            foreach (UserEntity existent in allOfThem) {
                context.Users.Remove(existent);
            }
            context.SaveChanges();
        }
    }
}

