using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataRepositoryInterfaces;
using DataAccess;
using BusinessLogic;
using RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace DataRepositories
{
    public class UserRepository : IUserRepository, IRepository<User>
    {
        private readonly ContextFactory creator;
        private DbContextOptions<DatabaseConnection> options;

        public UserRepository(ContextFactory aFactory)
        {
           creator = aFactory;
        }

        public UserRepository(DbContextOptions<DatabaseConnection> someOptions)
        {
            options = someOptions;
        }

        public void Add(User aUser){

        }
        public User GetUserByUsername(string aUsername)
        {
            return null;
        }

        public ICollection<User> GetAll(){
            return null;
        }
    }
}
