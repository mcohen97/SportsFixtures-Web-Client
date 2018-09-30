using System;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class UserServiceTest
    {
        private Mock<IUserRepository> users;
        private IUserService service;
        private User testUser;

        [TestInitialize]
        public void SetUp() {
            users = new Mock<IUserRepository>();
            service = new UserService(users.Object);
            testUser = GetFakeUser();
            users.Setup(r => r.Get("JohnDoe")).Returns(testUser);
            users.Setup(r => r.Get(It.Is<string>(s => !s.Equals("JohnDoe")))).Throws(new UserNotFoundException());

        }

        private User GetFakeUser()
        {
            UserId identity = new UserId
            {
                Name = "John",
                Surname = "Doe",
                UserName = "JohnDoe"
                ,
                Password = "Password",
                Email = "John@Doe.com"
            };
            return new User(identity, true);
        }

        [TestMethod]
        public void GetUserTest()
        {
            User retrieved = service.GetUser("JohnDoe");
            Assert.AreEqual(retrieved, testUser);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void GetNotExistsTest() {
            User retrieved = service.GetUser("JohnLennon");
        }

        [TestMethod]
        public void AddUserTest()
        {
        }

        [TestMethod]
        public void ModifyUserTest()
        {
        }

        [TestMethod]
        public void DeleteUserTest() {

        }
    }
}
