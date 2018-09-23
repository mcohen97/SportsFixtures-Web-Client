using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.Services;
using Moq;
using BusinessLogic;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DataRepositoryInterfaces;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class AuthControllerTest
    {
        private AuthenticationController controllerToTest;
        private Mock<ILoginService> logger;
        private Mock<User> testUser;

        [TestInitialize]
        public void StartUp() {
            logger = new Mock<ILoginService>();
            UserId identity = new UserId() { Name= "aName", Surname="aUsername", UserName="aUsername",
                Password= "aPassword", Email= "anEmail@aDomain.com" };
            testUser = new Mock<User>(identity,true);
            controllerToTest = new AuthenticationController(logger.Object);
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
