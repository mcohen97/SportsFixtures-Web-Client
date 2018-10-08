﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.Services;
using Moq;
using BusinessLogic;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DataRepositoryInterfaces;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Exceptions;
using System;

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

            //assert
            logger.VerifyAll();
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public void LoginNotFoundTest() {
            //arrange
            Exception toThrow = new UserNotFoundException();
            logger.Setup(l => l.Login("otherUsername", "aPassword")).Throws(toThrow);

            //act
            LoginModelIn credentials = new LoginModelIn() { Username = "otherUsername", Password = "aPassword" };
            IActionResult result = controllerToTest.Authenticate(credentials);
            BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;
            ErrorModelOut error = badRequestResult.Value as ErrorModelOut;

            //assert
            logger.VerifyAll();
            Assert.IsNotNull(badRequestResult);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void LoginWrongPasswordTest() {
            //arrange
            Exception toThrow = new WrongPasswordException();
            logger.Setup(l => l.Login("aUsername", "otherPassword")).Throws(toThrow);

            //act
            LoginModelIn credentials = new LoginModelIn() { Username = "aUsername", Password = "otherPassword" };
            IActionResult result = controllerToTest.Authenticate(credentials);
            BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;
            ErrorModelOut error = badRequestResult.Value as ErrorModelOut;

            //assert
            logger.VerifyAll();
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
