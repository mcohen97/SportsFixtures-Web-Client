using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.Services;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Exceptions;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
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
            User logged = logger.Login("aUsername", "aPassword");

            repo.VerifyAll();
            Assert.AreEqual(logged.Name, "aName");
            Assert.AreEqual(logged.Surname, "aSurname");
            Assert.AreEqual(logged.UserName, "aUsername");
            Assert.AreEqual(logged.Password, "aPassword");
            Assert.AreEqual(logged.Email, "anEmail@aDomain.com");
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
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
            User logged = logger.Login("aUsername", "otherPassword");
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
        [ExpectedException(typeof(UserNotFoundException))]
        public void SetUnexistentUserTest() {
            //Arrange.
            repo.Setup(r => r.Get(It.IsAny<string>())).Throws(new UserNotFoundException());
            //Act.
            logger.SetSession(admin.UserName);
        }

        [TestMethod]
        public void HasAdminPermissionTest() {
            //Arrange.
            repo.Setup(r => r.Get(admin.UserName)).Returns(admin);
            //Act.
            logger.SetSession(admin.UserName);
            //Assert.
            Assert.IsTrue(logger.HasAdminPermissions());   
        }

        [TestMethod]
        public void HasNoAdminPermissionsTest()
        {
            //Arrange.
            repo.Setup(r => r.Get(admin.UserName)).Returns(follower);
            //Act.
            logger.SetSession(follower.UserName);
            //Assert.
            Assert.IsFalse(logger.HasAdminPermissions());
        }
    }
}
