using BusinessLogic;
using DataRepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class LoginService
    {
        private IUserRepository users;

        public LoginService(IUserRepository aRepo)
        {
            users = aRepo;
        }

        public User Login(string aUsername, string aPassword)
        {
            User fetched = users.GetUserByUsername(aUsername);
            return fetched;
        }
    }
}
