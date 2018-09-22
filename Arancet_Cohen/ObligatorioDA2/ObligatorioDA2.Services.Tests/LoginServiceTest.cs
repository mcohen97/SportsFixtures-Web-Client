using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.Services;

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
            Assert.AreEqual(logged.Name,"aName");
            Assert.AreEqual(logged.Surname,"aSurname");
            Assert.AreEqual(logged.UserName,"aUsername");
            Assert.AreEqual(logged.Password,"aPassword");
            Assert.AreEqual(logged.Email, "anEmail@aDomain.com");
        }


    }
}
