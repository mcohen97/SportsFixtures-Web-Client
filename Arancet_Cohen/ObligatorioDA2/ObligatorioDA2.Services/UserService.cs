using BusinessLogic;
using DataRepositoryInterfaces;
using ObligatorioDA2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class UserService:IUserService
    {
        private IUserRepository usersStorage;

        public UserService(IUserRepository repository) {
            usersStorage = repository;
        }

        public User GetUser(string username)
        {
            return usersStorage.Get(username);
        }
    }
}
