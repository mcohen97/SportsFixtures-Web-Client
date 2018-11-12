using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.Services;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AuthenticationServiceTest
    {

        private Mock<IUserRepository> repo;
        private AuthenticationService logger;
        private User admin;
        private User follower;

        [TestInitialize]
        public void SetUp()
        {
            repo = new Mock<IUserRepository>();
            logger = new AuthenticationService(repo.Object);
            UserId id = new UserId
            {
                Name = "aName",
                Surname = "aSurname",
                Password = "aPassword",
                UserName = "aUsername",
                Email = "anEmail@aDomain.com"
            };
            follower = new User(id, false);
            admin = new User(id, true);
        }

        [TestMethod]
        public void LoginSuccesfullyTest()
        {
            //arrange
            repo.Setup(r => r.Get("aUsername")).Returns(admin);

            //act
            UserDto logged = logger.Login("aUsername", "aPassword");

            repo.VerifyAll();
            Assert.AreEqual(logged.name, "aName");
            Assert.AreEqual(logged.surname, "aSurname");
            Assert.AreEqual(logged.username, "aUsername");
            Assert.AreEqual(logged.password, "aPassword");
            Assert.AreEqual(logged.email, "anEmail@aDomain.com");
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void UserNotFoundTest()
        {
            //arrange.
            repo.Setup(r => r.Get("otherUsername")).Throws(new UserNotFoundException());
            //act.
            logger.Login("otherUsername", "password");
        }

        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordTest()
        {
            //arrange
            repo.Setup(r => r.Get("aUsername")).Returns(admin);

            //act
            UserDto logged = logger.Login("aUsername", "otherPassword");
        }

        [TestMethod]
        public void SetConnectedUserTest() {
            //Arrange.
            repo.Setup(r => r.Get(admin.UserName)).Returns(admin);
            //Act.
            logger.SetSession(admin.UserName);
            //Assert.
            repo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void SetUnexistentUserTest() {
            //Arrange.
            repo.Setup(r => r.Get(It.IsAny<string>())).Throws(new UserNotFoundException());
            //Act.
            logger.SetSession(admin.UserName);
        }

        [TestMethod]
        public void IsLoggedInTest() {
            repo.Setup(r => r.Get("aUsername")).Returns(admin);
            logger.SetSession("aUsername");
            repo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]
        public void IsNotLoggedInTest() {
            repo.Setup(r => r.Get("aUsername")).Throws(new UserNotFoundException());
            logger.Authenticate();
        } 

        [TestMethod]
        public void HasAdminPermissionTest() {
            //Arrange.
            repo.Setup(r => r.Get(admin.UserName)).Returns(admin);
            //Act.
            logger.SetSession(admin.UserName);
            //Assert.
            logger.AuthenticateAdmin();
            repo.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(NoPermissionsException))]
        public void HasNoAdminPermissionsTest()
        {
            //Arrange.
            repo.Setup(r => r.Get(admin.UserName)).Returns(follower);
            //Act.
            logger.SetSession(follower.UserName);
            //Assert.
            logger.AuthenticateAdmin();
        }
    }
}
