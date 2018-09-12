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

namespace DataAccessTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        IUserRepository usersStorage;

        [TestInitialize]
        public void SetUp() {
            Mock<User> user1 = new Mock<User>();
            Mock<User> user2 = new Mock<User>();
            Mock<User> user3 = new Mock<User>();

            user1.Setup(u => u.UserName).Returns("user1");
            user1.Setup(u => u.Name).Returns("name1");
            user2.Setup(u => u.UserName).Returns("user2");
            user3.Setup(u => u.UserName).Returns("user3");

            ICollection<Mock<User>> users = new List<Mock<User>>() {user1,user2,user3};
            Mock<DbSet<User>> mockSet = new Mock<DbSet<User>>();
            mockSet.Object.AddRange(users.Select(m=>m.Object));
            

            Mock<DatabaseConnection> fakeDB = new Mock<DatabaseConnection>();
            fakeDB.Setup(db => db.Users).Returns(mockSet.Object);

            ContextFactory factory = new ContextFactory();
            factory.Register<DatabaseConnection>(fakeDB.Object);
                        
            usersStorage = new UserRepository(factory);
        }
        [TestMethod]
        public void GetUserTest()
        {
            User queried = usersStorage.GetUserByUsername("user1");
            Assert.AreEqual("name1", queried.Name);
        }
    }
}
