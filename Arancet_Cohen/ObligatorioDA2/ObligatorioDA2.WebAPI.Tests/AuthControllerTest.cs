using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.Services;
using Moq;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class AuthControllerTest
    {
        private AuthController controllerToTest;
        private Mock<LoginService> logger;
        private Mock<User> testUser;
        [TestInitialize]
        public void StartUp() {
            logger = new Mock<LoginService>();
            UserId identity = new UserId() { Name= "aName", Surname="aUsername", UserName="aUsername",
                Password= "aPassword", Email= "anEmail@aDomain.com" };
            testUser = new Mock<User>(identity);
        }

        [TestMethod]
        public void LoginSuccesfullyTest() {
            //arrange
            logger.Setup(l => l.Login("aUsername", "aPassword")).Returns(testUser.Object);

            //act
            LoginModelIn credentials = new LoginModelIn() { Username = "aUsername", Password = "aPassword" };
            IActionResult result = controllerToTest.Authenticate(credentials);
            OkObjectResult okResult = result as OkObjectResult;

            logger.VerifyAll();
            Assert.IsNotNull(okResult);
        }
    }
}
