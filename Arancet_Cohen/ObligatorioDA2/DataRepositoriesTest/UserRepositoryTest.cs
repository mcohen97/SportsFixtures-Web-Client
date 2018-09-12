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
            Mock<User> user1 = new Mock<User>("name1", "surname1", "username1", "password1", "mail@domain.com");
            Mock<User> user2 = new Mock<User>("name2", "surname2", "username2", "password2", "mail@domain.com");
            Mock<User> user3 = new Mock<User>("name3", "surname3", "username3", "password3", "mail@domain.com");

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
