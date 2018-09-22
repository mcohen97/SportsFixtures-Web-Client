using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using DataAccess;
using DataRepositoryInterfaces;
using DataRepositories;
using System.Collections.Generic;
using BusinessLogic;
using BusinessLogic.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using RepositoryInterface;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.DataAccess.Domain.Mappers;

namespace DataRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IEntityRepository<UserEntity> repo;
        private readonly UserMapper mapper;

        public UserRepository(IEntityRepository<UserEntity> genericRepo)
        {
            repo = genericRepo;
            mapper = new UserMapper();
        }

        public void Add(User aUser)
        {
            if (!AnyWithThisUserName(aUser.UserName))
            {
                AddNewUser(aUser);
            }
            else
            {
                throw new UserAlreadyExistsException();
            }
        }

        private void AddNewUser(User aUser)
        {
            UserEntity toAdd = mapper.ToEntity(aUser);
            repo.Add(toAdd);
        }

        public User GetUserByUsername(string aUserName)
        {
            User toReturn;
            if (AnyWithThisUserName(aUserName))
            {
                toReturn = GetExistentUser(aUserName);
            }
            else
            {
                throw new UserNotFoundException();
            }
            return toReturn;
        }

        private User GetExistentUser(string aUserName)
        {
            UserEntity fetched = repo.Get(u => u.UserName.Equals(aUserName)).First();
            //UserEntity fetched = connection.Users.First(u => u.UserName.Equals(aUserName));
            User toReturn = mapper.ToUser(fetched);
            return toReturn;
        }

        public ICollection<User> GetAll()
        {
            ICollection<UserEntity> query = repo.GetAll();
            ICollection<User> users = query.Select(u => mapper.ToUser(u)).ToList();
            //.Select(u => mapper.ToUser(u));

            //IQueryable<User> query = connection.Users.Select(u => mapper.ToUser(u));
            return users;
        }

        public bool IsEmpty()
        {
            return repo.IsEmpty();
            //return !connection.Users.Any();
        }

        public void Delete(User entity)
        {
            if (AnyWithThisUserName(entity.UserName))
            {
                int generatedId = GetUserByUsername(entity.UserName).Id;
                repo.Delete(entity.Id);
                /* UserEntity toDelete = connection.Users.First(r => r.UserName.Equals(entity.UserName));
                 connection.Users.Remove(toDelete);
                 connection.SaveChanges();*/
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        public void Delete(int identity)
        {
            if (repo.Any(r => r.Id.Equals(identity)))
            {
                repo.Delete(identity);
                /*UserEntity toDelete = connection.Users.First(r => r.Id.Equals(identity));
                connection.Users.Remove(toDelete);
                connection.SaveChanges();*/
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        public bool Exists(User entity)
        {
            UserEntity record = mapper.ToEntity(entity);
            bool doesExist = repo.Exists(record);
            //bool doesExist = AnyWithThisUserName(record.UserName);
            return doesExist;
        }

        public void Modify(User aUser)
        {
            if (AnyWithThisUserName(aUser.UserName))
            {
                UserEntity entity = mapper.ToEntity(aUser);
                repo.Modify(entity);
                /*UserEntity toModify = connection.Users.First(u => u.UserName.Equals(aUser.UserName));
                UserEntity newRecord = mapper.ToEntity(aUser);
                newRecord.Id = toModify.Id;
                connection.Entry(toModify).CurrentValues.SetValues(newRecord);
                connection.SaveChanges();*/
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        private bool AnyWithThisUserName(string userName)
        {
            return repo.Any(u => u.UserName.Equals(userName));
        }

        public User Get(User asked)
        {
            return GetUserByUsername(asked.UserName);
        }

        public User Get(int anId)
        {
            User query;
            bool exists = repo.Any(u => u.Id == anId);
            if (exists)
            {
                UserEntity record = repo.First(u => u.Id == anId);
                query = mapper.ToUser(record);
            }
            else
            {
                throw new UserNotFoundException();
            }
            return query;
        }

        public void Clear()
        {
            repo.Clear();
            //foreach (UserEntity user in connection.Users)
            //{
            //    connection.Users.Remove(user);
            //}
            //connection.SaveChanges();
        }
    }
}
