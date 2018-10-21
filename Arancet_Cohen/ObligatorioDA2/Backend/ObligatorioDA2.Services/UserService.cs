using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class UserService : IUserService
    {
        private IUserRepository usersStorage;
        private ITeamRepository teamsStorage;

        public UserService(IUserRepository usersRepository, ITeamRepository teamRepository)
        {
            usersStorage = usersRepository;
            teamsStorage = teamRepository;
        }

        public void AddUser(User testUser)
        {
            usersStorage.Add(testUser);
        }

        public void DeleteUser(string userName)
        {
            usersStorage.Delete(userName);
        }

        public User GetUser(string username)
        {
            return usersStorage.Get(username);
        }

        public void ModifyUser(User testUser)
        {
            usersStorage.Modify(testUser);
        }

        public void FollowTeam(string userName, int idTeam)
        {
            Team toFollow = teamsStorage.Get(idTeam);
            FollowTeam(userName, toFollow);
        }

        public void FollowTeam(string username, Team toFollow)
        {
            try
            {
                TryFollowTeam(username, toFollow);
            }
            catch (InvalidUserDataException)
            {
                throw new TeamAlreadyFollowedException();
            }
        }

        private void TryFollowTeam(string username, Team toFollow)
        {
            User follower = usersStorage.Get(username);
            follower.AddFavourite(toFollow);
            usersStorage.Modify(follower);
        }

        public void UnFollowTeam(string userName, int idTeam)
        {
            Team toUnfollow = teamsStorage.Get(idTeam);
            UnFollowTeam(userName, toUnfollow);
        }

        public void UnFollowTeam(string username, Team fake)
        {
            try
            {
                TryUnFollow(username, fake);
            }
            catch (InvalidUserDataException)
            {
                throw new TeamNotFollowedException();
            }
        }

        private void TryUnFollow(string username, Team fake)
        {
            User follower = usersStorage.Get(username);
            follower.RemoveFavouriteTeam(fake);
            usersStorage.Modify(follower);
        }


        public ICollection<Team> GetUserTeams(string userName)
        {
            User fetched = usersStorage.Get(userName);
            return fetched.GetFavouriteTeams();
        }

        public ICollection<User> GetAllUsers()
        {
            return usersStorage.GetAll();
        }

    }
}
