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
        private readonly UserMapper userMapper;
        private readonly TeamMapper teamMapper;

        public UserRepository(DatabaseConnection aContext)
        {
            context = aContext;
            userMapper = new UserMapper();
            teamMapper = new TeamMapper();
        }

        public User Add(User aUser)
        {
            User added;
            if (!AnyWithThisUserName(aUser.UserName))
            {
                UserEntity entity = userMapper.ToEntity(aUser);
                ICollection<UserTeam> follower_teams = userMapper.GetUserTeams(aUser);
                context.Users.Add(entity);
                context.UserTeams.AddRange(follower_teams);
                context.SaveChanges();
                added = aUser;
            }
            else
            {
                throw new UserAlreadyExistsException();
            }
            return added;
        }

        public User Get(string aUserName)
        {
            User toReturn;
            if (AnyWithThisUserName(aUserName))
            {
                UserEntity retrieved = GetEntityByUsername(aUserName);
                ICollection<TeamEntity> teams = GetFollowedTeams(aUserName); 
                toReturn = userMapper.ToUser(retrieved,teams);
                context.Entry(retrieved).State = EntityState.Detached;
            }
            else
            {
                throw new UserNotFoundException();
            }
            return toReturn;
        }

        private ICollection<TeamEntity> GetFollowedTeams(string aUserName)
        {
           return context.UserTeams
                    .Where(ut => ut.UserEntityUserName.Equals(aUserName))
                    .Select(ut => ut.Team)
                    .ToList();
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
            ICollection<User> users = new List<User>();

            foreach (UserEntity user in query) {
                IQueryable<TeamEntity> followed = context.UserTeams
                    .Where(ue => ue.UserEntityUserName.Equals(user.UserName))
                    .Select(ue => ue.Team);
                User built = userMapper.ToUser(user, followed.ToList());
                users.Add(built);
            };
            return users;
        }

        public bool IsEmpty()
        {
            return !context.Users.Any();
        }

       

        public bool Exists(User entity)
        {
            UserEntity record = userMapper.ToEntity(entity);
            bool doesExist = context.Users.Any(u => u.UserName.Equals(entity.UserName));
            return doesExist;
        }

        public void Modify(User aUser)
        {
            if (AnyWithThisUserName(aUser.UserName))
            {
                UserEntity entity = userMapper.ToEntity(aUser);

                foreach (UserTeam team in userMapper.GetUserTeams(aUser)) {
                    if (!context.UserTeams.Any(ut=>ut.Team.Equals(team.Team) && ut.Follower.Equals(team.Follower))) {
                        context.Entry(team).State=EntityState.Added;
                    }
                }
                context.Update(entity);
                context.SaveChanges();
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

