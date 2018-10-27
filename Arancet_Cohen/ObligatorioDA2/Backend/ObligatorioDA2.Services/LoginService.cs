using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository users;
        private ILoggerService logger;

        public LoginService(IUserRepository aRepo, ILoggerService logService)
        {
            users = aRepo;
            logger = logService;
        }

        public User Login(string aUsername, string aPassword)
        {
            User fetched = users.Get(aUsername);
            if (!aPassword.Equals(fetched.Password))
            {
                throw new WrongPasswordException();
            }
            return fetched;
        }
    }
}
