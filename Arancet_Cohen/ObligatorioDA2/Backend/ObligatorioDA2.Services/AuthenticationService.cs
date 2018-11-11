using System;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;
using ObligatorioDA2.Services.Mappers;

namespace ObligatorioDA2.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUserRepository users;
        private User current;
        private UserDtoMapper mapper;

        public AuthenticationService(IUserRepository aRepo)
        {
            users = aRepo;
            mapper = new UserDtoMapper();
        }

        public UserDto Login(string aUsername, string aPassword)
        {
            User fetched = TryGetUser(aUsername); 
            if (!aPassword.Equals(fetched.Password))
            {
                throw new WrongPasswordException();
            }
            return mapper.ToDto(fetched);
        }

        private User TryGetUser(string aUsername)
        {
            try
            {
                return users.Get(aUsername);
            }
            catch (UserNotFoundException e) {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public void SetSession(string userName)
        {
            try
            {
                current = users.Get(userName);
            }
            catch (UserNotFoundException e) {
                throw new ServiceException(e.Message, ErrorType.NOT_AUTHENTICATED);
            }
        }

        public void AuthenticateAdmin()
        {
            if (!IsLoggedIn())
            {
                throw new NotAuthenticatedException();
            }

            if (!HasAdminPermissions())
            {
                throw new NoPermissionsException();
            }
        }

        public void Authenticate()
        {
            if (!IsLoggedIn())
            {
                throw new NotAuthenticatedException();
            }
        }
        private bool HasAdminPermissions()
        {
            return (current != null) && current.IsAdmin;
        }

        private bool IsLoggedIn()
        {
            return (current != null);
        }
    }
}
