using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using DataAccess;
using DataRepositoryInterfaces;
using DataRepositories;
using System.Collections.Generic;
using BusinessLogic;
using RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

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
            ContextFactory factory = new ContextFactory(options);
            usersStorage = new UserRepository(factory);
            ClearDatabase(factory);
        }

        private void ClearDatabase(ContextFactory factory)
        {
            using (DatabaseConnection db = factory.Get()) {
                foreach (User user in db.Users) {
                    db.Users.Remove(user);
                }
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void EmptyDbTest() {
            int expectedResult = 0;
            int actualResult = usersStorage.GetAll().Count;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void AddUserTest(){
            IRepository<User> generic = (IRepository<User>)usersStorage;
            Mock<User> user1 = new Mock<User>("name1", "surname1", "username1", "password1", "mail@domain.com");
            generic.Add(user1.Object);
            int expectedResult = 1;
            int actualResult = generic.GetAll().Count;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetUserTest()
        {
            IUserRepository specific = (IUserRepository)usersStorage;
            User queried = specific.GetUserByUsername("user1");
            Assert.AreEqual("name1", queried.Name);
        }
    }
}
