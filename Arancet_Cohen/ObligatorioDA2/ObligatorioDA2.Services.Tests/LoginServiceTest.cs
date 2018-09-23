using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.Services;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Exceptions;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class LoginServiceTest
    {

        private Mock<IUserRepository> repo;
        private LoginService logger;
        private User user;

        [TestInitialize]
        public void SetUp() {
            repo = new Mock<IUserRepository>();
            logger = new LoginService(repo.Object);
            UserId id = new UserId { Name = "aName", Surname = "aSurname",
                Password = "aPassword", UserName = "aUsername", Email = "anEmail@aDomain.com" };

            user = new User(id, true);
        }

        [TestMethod]
        public void LoginSuccesfullyTest() {
            //arrange
            repo.Setup(r => r.GetUserByUsername("aUsername")).Returns(user);

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
        public void UserNotFoundTest() {
            //arrange.
            repo.Setup(r => r.GetUserByUsername("otherUsername")).Throws(new UserNotFoundException());
            //act.
            logger.Login("otherUsername", "password");
        }

        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void WrongPasswordTest() {
            //arrange
            repo.Setup(r => r.GetUserByUsername("aUsername")).Returns(user);

            //act
            User logged = logger.Login("aUsername", "otherPassword");
        }
    }
}
