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
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

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
                foreach (UserEntity user in context.Users) {
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
            Mock<User> user = new Mock<User>("name", "surname", "username", "password", "mail@domain.com");
            usersStorage.Add(user.Object);
            int expectedResult=1;
            int actualResult = usersStorage.GetAll().Count;
            Assert.AreEqual(expectedResult,actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAlreadyExistsException))]
        public void AddAlreadyExistentUserTest() {
            Mock<User> user = new Mock<User>("name", "surname", "username", "password", "mail@domain.com");
            usersStorage.Add(user.Object);
            usersStorage.Add(user.Object);
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

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void GetNotExistentUserTest() {
            IUserRepository specific = (IUserRepository)usersStorage;
            User fetched = specific.GetUserByUsername("username3");
        }

        [TestMethod]
        public void ExistsUserTest() {
           Mock<User> user = new Mock<User>("name", "surname", "username", "password", "mail@domain.com");
            usersStorage.Add(user.Object);
            bool result = usersStorage.Exists(user.Object);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesNotExistTest() {
          Mock<User> user1 = new Mock<User>("name1", "surname1", "username1", "password1", "mail1@domain.com");
          Mock<User> user2 = new Mock<User>("name2", "surname2", "username2", "password2", "mail2@domain.com");
          usersStorage.Add(user1.Object);
          bool result = usersStorage.Exists(user2.Object);
          Assert.IsTrue(result);
        }
    }
}
