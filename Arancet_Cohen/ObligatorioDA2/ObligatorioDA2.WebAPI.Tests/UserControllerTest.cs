using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using DataRepositories;
using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.DataAccess.Entities;
using Moq;
using BusinessLogic;
using System;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Interfaces;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        UsersController controller;
        Mock<IUserService> service;
        UserModelIn input;

        [TestInitialize]
        public void SetUp()
        {

            service = new Mock<IUserService>();
            controller = new UsersController(service.Object);
            input = new UserModelIn() { Name = "James", Surname = "Hetfield", Username = "JHetfield63", Password = "password", Email = "JHetfield@gmail.com" };
        }

        [TestMethod]
        public void GetTest()
        {
            //Arrange.
            User fake = GetFakeUser();
            service.Setup(us => us.GetUser(fake.UserName)).Returns(fake);

            //Act.
            IActionResult result = controller.Get(fake.UserName);
            OkObjectResult okResult = result as OkObjectResult;
            UserModelOut modelOut = okResult.Value as UserModelOut;

            //Assert.
            service.Verify(us => us.GetUser(fake.UserName), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modelOut);
            Assert.AreEqual(modelOut.Username, fake.UserName);
        }

        [TestMethod]
        public void GetNotExistentTest()
        {
            //Arrange.
            Exception toThrow = new UserNotFoundException();
            service.Setup(us => us.GetUser(It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Get("username");
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            service.Verify(us => us.GetUser("username"), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void CreateValidUserTest()
        {          
            //Act.
            IActionResult result = controller.Post(input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            UserModelOut modelOut = createdResult.Value as UserModelOut;

            //Assert.
            service.Verify(us => us.AddUser(It.IsAny<User>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual("GetUserById", createdResult.RouteName);
            Assert.AreEqual(modelOut.Username, input.Username);
        }
      
        [TestMethod]
        public void CreateFailedUserTest()
        {
            //Arrange.
            var modelIn = new UserModelIn()
            {
                Name = "name",
                Surname = "surname",
                Password = "password"
            };
            //We need to force the error in de ModelState.
            controller.ModelState.AddModelError("", "Error");

            //Act.
            IActionResult result = controller.Post(modelIn);            
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;

            //Assert.
            service.Verify(us => us.AddUser(It.IsAny<User>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void CreateAlreadyExistentUserTest() {
            //Arrange.
            Exception toThrow = new UserAlreadyExistsException();
            service.Setup(us => us.AddUser(It.IsAny<User>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Post(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert.
            service.Verify(us => us.AddUser(It.IsAny<User>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }


        [TestMethod]
        public void PutModifyTest()
        {
            //Arrange.
            var modelIn = new UserModelIn()
            {
                Name = "name1",
                Surname = "surname1",
                Username = "username",
                Password = "password1",
                Email = "mail@domain.com"
            };

            //Act.
            IActionResult result = controller.Put(modelIn.Username,modelIn);
            OkObjectResult okResult = result as OkObjectResult;
            UserModelOut modified = okResult.Value as UserModelOut;

            //Assert.
            service.Verify(us => us.ModifyUser(It.IsAny<User>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(modified);
        }

        [TestMethod]
        public void PutCreateTest()
        {
            //Arrange.
            var modelIn = new UserModelIn()
            {
                Name = "name1",
                Surname = "surname1",
                Username = "username",
                Password = "password1",
                Email = "mail@domain.com"
            };
            service.Setup(us => us.ModifyUser(It.IsAny<User>())).Throws(new UserNotFoundException());

            //Act.
            IActionResult result = controller.Put("username", modelIn);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            UserModelOut added = createdResult.Value as UserModelOut;

            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.IsNotNull(added);
            Assert.AreEqual("GetUserById", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(modelIn.Username,added.Username);
        }

        [TestMethod]
        public void PutBadFormatTest() {
            var model = new UserModelIn()
            {
                Surname = "surname1",
                Password = "password1",
                Email = "mail@domain.com"
            };
            controller.ModelState.AddModelError("", "Error");
            IActionResult result =controller.Put("username", model);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
        }

        [TestMethod]
        public void DeleteTest()
        {
            //Act.
            IActionResult result =controller.Delete("username");
            OkResult deletedResult = result as OkResult;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(deletedResult);
            Assert.AreEqual(200, deletedResult.StatusCode);
        }

        [TestMethod]
        public void DeleteNotExistentTest()
        {
            //Arrange.
            Exception toThrow = new UserNotFoundException();
            service.Setup(ms => ms.DeleteUser("notExistent")).Throws(toThrow);

            //Act.
            IActionResult result = controller.Delete("notExistent");
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        private User GetFakeUser() {
            UserId identity = new UserId()
            {
                Name = "name",
                Surname = "surname",
                UserName = "username",
                Password = "password",
                Email = "mail@mail.com"
            };
            User created = new User(identity, false);
            return created;
        }
    }
}