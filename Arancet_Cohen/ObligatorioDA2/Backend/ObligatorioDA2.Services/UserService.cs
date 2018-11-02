using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.BusinessLogic.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class UserService : IUserService
    {
        private IUserRepository usersStorage;
        private ITeamRepository teamsStorage;
        private UserFactory factory;

        public UserService(IUserRepository usersRepository, ITeamRepository teamRepository)
        {
            usersStorage = usersRepository;
            teamsStorage = teamRepository;
            factory = new UserFactory();
        }

        public User AddUser(UserDto userData)
        {
            User toAdd= TryCreate(userData);
            TryAdd(toAdd);
            return toAdd;
        }

        private User TryCreate(UserDto userData)
        {
            User toCreate;
            try
            {
                toCreate = CreateUser(userData);
            }
            catch (InvalidUserDataException e)
            {
                throw new ServiceException(e.Message, ErrorType.INVALID_DATA);
            }
            return toCreate;
        }

        private void TryAdd(User toAdd)
        {
            try
            {
                usersStorage.Add(toAdd);
            }
            catch (EntityAlreadyExistsException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_ALREADY_EXISTS);
            }
            catch (EntityNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        private User CreateUser(UserDto userData)
        {
            UserId identity = new UserId()
            {
                Name = userData.email,
                Surname = userData.surname,
                UserName = userData.username,
                Password = userData.password,
                Email = userData.email
            };

            User created = userData.isAdmin ? factory.CreateAdmin(identity) : factory.CreateFollower(identity);

            return created;
        }

        public void DeleteUser(string userName)
        {
            try
            {
                usersStorage.Delete(userName);
            }
            catch (EntityNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public User GetUser(string username)
        {
            try
            {
                return usersStorage.Get(username);
            }
            catch (EntityNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e)
            {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public User ModifyUser(UserDto toModify)
        {
            User old = GetUser(toModify.username);
            User updated = TryUpdate(old, toModify);
            try
            {
                usersStorage.Modify(updated);
            }
            catch (EntityNotFoundException e) {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return updated;
       }

        private User TryUpdate(User old, UserDto toModify)
        {
            User updated;
            try
            {
                updated = UpdateUserData(old, toModify);
            }
            catch (InvalidUserDataException e) {
                throw new ServiceException(e.Message, ErrorType.INVALID_DATA);
            }
            return updated;
        }

        private User UpdateUserData(User old, UserDto toModify)
        {
            string newName = string.IsNullOrWhiteSpace(toModify.name) ? old.Name : toModify.name;
            string newSurname = string.IsNullOrWhiteSpace(toModify.surname) ? old.Surname : toModify.surname;
            string newUsername = string.IsNullOrWhiteSpace(toModify.username) ? old.UserName : toModify.username;
            string newPassword = string.IsNullOrWhiteSpace(toModify.password) ? old.Password : toModify.password;
            string newEmail = string.IsNullOrWhiteSpace(toModify.email) ? old.Email : toModify.email;

            UserId data = new UserId() {
                Name = newName,Surname = newSurname,UserName = newUsername,Email = newEmail,Password = newPassword};
            User updated = toModify.isAdmin ? factory.CreateAdmin(data) : factory.CreateFollower(data);
            return updated;
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
            ICollection<Team> userTeams;
            try
            {
                User fetched = usersStorage.Get(userName);
                userTeams = fetched.GetFavouriteTeams();
            }
            catch (EntityNotFoundException e) {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }catch(DataInaccessibleException e)
            {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return userTeams;
        }

        public ICollection<User> GetAllUsers()
        {
            ICollection<User> allOfThem;
            try
            {
                allOfThem = usersStorage.GetAll();
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return allOfThem;
        }

    }
}
