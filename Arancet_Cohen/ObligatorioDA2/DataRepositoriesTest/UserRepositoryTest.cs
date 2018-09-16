using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using DataAccess;
using DataRepositoryInterfaces;
using DataRepositories;
using System.Collections.Generic;
using BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using RepositoryInterface;
using ObligatorioDA2.DataAccess.Entities;

namespace DataAccessTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        IRepository<User> usersStorage;

        [TestInitialize]
        public void SetUp() {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "UserRepository")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            usersStorage = new UserRepository(context);
            ClearDataBase(context);
        }

        private void ClearDataBase(DatabaseConnection context)
        { 
                foreach (User user in context.Users) {
                    context.Users.Remove(user);
                }
                context.SaveChanges();  
        }

        [TestMethod]
        public void NoUsersTest() {
            int expectedResult = 0;
            int actualResult = usersStorage.GetAll().Count;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void AddUserTest(){
            Mock<User> user3 = new Mock<User>("name3", "surname3", "username3", "password3", "mail@domain.com");
            usersStorage.Add(user3.Object);
            int expectedResult=1;
            int actualResult = usersStorage.GetAll().Count;
            Assert.AreEqual(expectedResult,actualResult);
        }

        [TestMethod]
        public void GetUserTest()
        {
            IUserRepository specific = (IUserRepository)usersStorage;
            Mock<User> user3 = new Mock<User>("name3", "surname3", "username3", "password3", "mail@domain.com");
            usersStorage.Add(user3.Object);
            User fetched = specific.GetUserByUsername("username3");
            Assert.AreEqual("name3", fetched.Name);
        }
    }
}
