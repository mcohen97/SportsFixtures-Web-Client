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

namespace DataAccessTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        IUserRepository usersStorage;

        [TestInitialize]
        public void SetUp() {
            var options = new DbContextOptionsBuilder<DatabaseConnection>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            usersStorage= new UserRepository(options);
        }

        [TestMethod]
        public void AddUserTest(){
            Mock<User> user3 = new Mock<User>("name3", "surname3", "username3", "password3", "mail@domain.com");
            usersStorage.AddUser(user3.Object);
            int expectedResult=1;
            int actualResult = usersStorage.GetUsers().Count;
            Assert.AreEqual(expectedResult,actualResult);
        }

        [TestMethod]
        public void GetUserTest()
        {
            User queried = usersStorage.GetUserByUsername("user1");
            Assert.AreEqual("name1", queried.Name);
        }
    }
}
