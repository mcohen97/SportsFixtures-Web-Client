using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.Services.Interfaces;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Exceptions;
using System;
using ObligatorioDA2.Services.Interfaces.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AuthControllerTest
    {
        private AuthenticationController controllerToTest;
        private Mock<IAuthenticationService> loginService;
        private Mock<ILoggerService> logger;
        private UserDto testUser;

        [TestInitialize]
        public void StartUp() {
            loginService = new Mock<IAuthenticationService>();
            logger = new Mock<ILoggerService>();
            testUser = new UserDto() { name ="aName",surname ="aUsername",username = "aUsername",
                password = "aPassword",email= "anEmail@aDomain.com", isAdmin=true };
            controllerToTest = new AuthenticationController(loginService.Object, logger.Object);
        }

        [TestMethod]
        public void LoginSuccesfullyTest() {
            //arrange
            loginService.Setup(l => l.Login("aUsername", "aPassword")).Returns(testUser);
            logger.Setup(l => l.Log(LogType.LOGIN, It.IsAny<string>(), "aUsername", It.IsAny<DateTime>())).Returns(1);

            //act
            LoginModelIn credentials = new LoginModelIn() { Username = "aUsername", Password = "aPassword" };
            IActionResult result = controllerToTest.Authenticate(credentials);
            OkObjectResult okResult = result as OkObjectResult;

            //assert
            logger.Verify(l => l.Log(LogType.LOGIN, It.IsAny<string>(), "aUsername", It.IsAny<DateTime>()), Times.Once);
            Assert.IsNotNull(okResult);
            logger.Verify();
        }

        [TestMethod]
        public void LoginNotFoundTest() {
            //Arrange.
            Exception internalEx = new UserNotFoundException();
            Exception toThrow = new ServiceException(internalEx.Message, ErrorType.ENTITY_NOT_FOUND);
            loginService.Setup(l => l.Login("otherUsername", "aPassword")).Throws(toThrow);
            logger.Setup(l => l.Log(LogType.LOGIN, It.IsAny<string>(), "otherUsername", It.IsAny<DateTime>())).Returns(1);

            //Act.
            LoginModelIn credentials = new LoginModelIn() { Username = "otherUsername", Password = "aPassword" };
            IActionResult result = controllerToTest.Authenticate(credentials);
            BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;
            ErrorModelOut error = badRequestResult.Value as ErrorModelOut;

            //Assert.
            loginService.VerifyAll();
            logger.Setup(l => l.Log(LogType.LOGIN, It.IsAny<string>(), "aUsername", It.IsAny<DateTime>())).Returns(1);
            Assert.IsNotNull(badRequestResult);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void LoginNoDataAccessTest()
        {
            //Arrange.
            Exception internalEx = new DataInaccessibleException();
            Exception toThrow = new ServiceException(internalEx.Message, ErrorType.DATA_INACCESSIBLE);
            loginService.Setup(us => us.Login(It.IsAny<string>(),It.IsAny<string>())).Throws(toThrow);
            LoginModelIn credentials = new LoginModelIn() { Username = "otherUsername", Password = "aPassword" };
            logger.Setup(l => l.Log(LogType.LOGIN, It.IsAny<string>(), "otherUsername", It.IsAny<DateTime>())).Returns(1);

            //Act.
            IActionResult result = controllerToTest.Authenticate(credentials);
            ObjectResult noData = result as ObjectResult;
            ErrorModelOut error = noData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(noData);
            Assert.AreEqual(500, noData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void LoginWrongPasswordTest() {
            //arrange
            Exception toThrow = new WrongPasswordException();
            loginService.Setup(l => l.Login("aUsername", "otherPassword")).Throws(toThrow);
            logger.Setup(l => l.Log(LogType.LOGIN, It.IsAny<string>(), "aUsername", It.IsAny<DateTime>())).Returns(1);

            //act
            LoginModelIn credentials = new LoginModelIn() { Username = "aUsername", Password = "otherPassword" };
            IActionResult result = controllerToTest.Authenticate(credentials);
            BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;
            ErrorModelOut error = badRequestResult.Value as ErrorModelOut;

            //assert
            loginService.VerifyAll();
            Assert.IsNotNull(badRequestResult);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);
        }

        [TestMethod]
        public void LoginInvalidModelTest() {
            LoginModelIn credentials = new LoginModelIn();
            controllerToTest.ModelState.AddModelError("", "Error");
            IActionResult result = controllerToTest.Authenticate(credentials);
            BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
        }
    }
}
