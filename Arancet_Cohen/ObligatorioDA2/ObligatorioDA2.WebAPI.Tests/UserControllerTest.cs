using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.Data.Entities;
using Moq;
using ObligatorioDA2.BusinessLogic;
using System;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using ObligatorioDA2.Services.Exceptions;

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
        public void GetNoDataAccessTest() {
            
             //Arrange.
             Exception toThrow = new DataInaccessibleException();
            service.Setup(us => us.GetUser(It.IsAny<string>())).Throws(toThrow);


            //Act.
            IActionResult result = controller.Get("username");
            ObjectResult noData = result as ObjectResult;
            ErrorModelOut error = noData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(noData);
            Assert.AreEqual(500,noData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);
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
        public void CreateAlreadyExistentUserTest()
        {
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
        public void CreateUserNoDataAccessTest()
        {
            
             //Arrange.
             Exception toThrow = new DataInaccessibleException();
             service.Setup(us => us.AddUser(It.IsAny<User>())).Throws(toThrow);


            //Act.
            IActionResult result = controller.Post(input);
            ObjectResult noData = result as ObjectResult;
            ErrorModelOut error = noData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(noData);
            Assert.AreEqual(500,noData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);
           
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
            IActionResult result = controller.Put(modelIn.Username, modelIn);
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
            Assert.AreEqual(modelIn.Username, added.Username);
        }

        [TestMethod]
        public void PutBadFormatTest()
        {
            var model = new UserModelIn()
            {
                Surname = "surname1",
                Password = "password1",
                Email = "mail@domain.com"
            };
            controller.ModelState.AddModelError("", "Error");
            IActionResult result = controller.Put("username", model);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
        }

        [TestMethod]
        public void PutUserNoDataAccessTest()
        {

            //Arrange.
            UserModelIn modelIn = new UserModelIn()
            {
                Name = "name1",
                Surname = "surname1",
                Username = "username",
                Password = "password1",
                Email = "mail@domain.com"
            };
            Exception toThrow = new DataInaccessibleException();
            service.Setup(us => us.ModifyUser(It.IsAny<User>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Put("username", modelIn);
            ObjectResult noData = result as ObjectResult;
            ErrorModelOut error = noData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(noData);
            Assert.AreEqual(500,noData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);
           
        }

        [TestMethod]
        public void DeleteTest()
        {
            //Act.
            IActionResult result = controller.Delete("username");
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

        [TestMethod]
        public void DeleteUserNoDataAccessTest()
        { 
            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            service.Setup(us => us.DeleteUser(It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Delete("username");
            ObjectResult notData = result as ObjectResult;
            ErrorModelOut error = notData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(notData);
            Assert.AreEqual(500,notData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);
           
        }

        [TestMethod]
        public void GetAllTest()
        {
            //Arrange.
            ICollection<User> fakeList = new List<User>() { GetFakeUser(), GetFakeUser(), GetFakeUser() };
            service.Setup(us => us.GetAllUsers()).Returns(fakeList);

            //Act.
            IActionResult result = controller.Get();
            OkObjectResult listResult = result as OkObjectResult;
            ICollection<UserModelOut> list = listResult.Value as ICollection<UserModelOut>;

            //Assert.
            service.Verify(us => us.GetAllUsers(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(listResult);
            Assert.AreEqual(200, listResult.StatusCode);
            Assert.IsNotNull(list);
            Assert.AreEqual(fakeList.Count, list.Count);
        }

        [TestMethod]
        public void GetAllUsersNoDataAccessTest()
        {
            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            service.Setup(us => us.GetAllUsers()).Throws(toThrow);

            //Act.
            IActionResult result = controller.Get();
            ObjectResult notData = result as ObjectResult;
            ErrorModelOut error = notData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(notData);
            Assert.AreEqual(500,notData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);

        }

        [TestMethod]
        public void FollowTeamTest()
        {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            TeamModelIn input = GetTeamModelIn();

            //Act.
            IActionResult result = controller.FollowTeam(input);
            OkObjectResult okResult = result as OkObjectResult;
            OkModelOut okMessage = okResult.Value as OkModelOut;

            //Assert.
            service.Verify(us => us.FollowTeam("username", 3));
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200,okResult.StatusCode);
            Assert.IsNotNull(okMessage);
        }

        [TestMethod]
        public void FollowTeamAlreadyFollowing() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;

            Exception toThrow = new TeamAlreadyFollowedException();
            service.Setup(us => us.FollowTeam(It.IsAny<string>(), It.IsAny<int>())).Throws(toThrow);
            TeamModelIn input = GetTeamModelIn();

            //Act.
            IActionResult result = controller.FollowTeam(input);
            BadRequestObjectResult notFound = result as BadRequestObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            service.Verify(us => us.FollowTeam(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(400, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void FollowTeamNotExistentTest() {

            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;

            Exception toThrow = new TeamNotFoundException();
            service.Setup(us => us.FollowTeam(It.IsAny<string>(), It.IsAny<int>())).Throws(toThrow);
            TeamModelIn input = GetTeamModelIn();

            //Act.
            IActionResult result = controller.FollowTeam(input);
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            service.Verify(us => us.FollowTeam(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void FollowTeamInvalidFormatTest() {

            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            controller.ModelState.AddModelError("", "Error");
            TeamModelIn input = new TeamModelIn() { };

            //Act.
            IActionResult result =controller.FollowTeam(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;

            //Assert.
            service.Verify(us => us.FollowTeam(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400,badRequest.StatusCode);
        }

        [TestMethod]
        public void FollowTeamNoDataAccessTest()
        {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            Exception toThrow = new DataInaccessibleException();
            TeamModelIn input = GetTeamModelIn();
            service.Setup(us => us.FollowTeam(It.IsAny<string>(), It.IsAny<int>())).Throws(toThrow);


            //Act.
            IActionResult result = controller.FollowTeam(input);
            ObjectResult noData = result as ObjectResult;
            ErrorModelOut error = noData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(noData);
            Assert.AreEqual(500,noData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);
           
        }

        [TestMethod]
        public void UnFollowTeamTest() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            TeamModelIn input = GetTeamModelIn();

            //Act.
            IActionResult result = controller.UnFollowTeam(input);
            OkObjectResult okResult = result as OkObjectResult;
            OkModelOut okModel = okResult.Value as OkModelOut;

            //Assert.
            service.Verify(us => us.UnFollowTeam(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okModel);
        }

        [TestMethod]
        public void UnfollowTeamNotFoundTest() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            TeamModelIn input = GetTeamModelIn();
            Exception toThrow = new TeamNotFoundException();
            service.Setup(us => us.UnFollowTeam(It.IsAny<string>(), It.IsAny<int>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.UnFollowTeam(input);
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            service.Verify(us => us.UnFollowTeam(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void UnfollowNotFollowedTest() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            TeamModelIn input = GetTeamModelIn();
            Exception toThrow = new TeamNotFollowedException();
            service.Setup(us => us.UnFollowTeam(It.IsAny<string>(), It.IsAny<int>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.UnFollowTeam(input);
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            service.Verify(us => us.UnFollowTeam(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void UnfollowInvalidTeamTest() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            controller.ModelState.AddModelError("", "Error");
            TeamModelIn input = new TeamModelIn() { };

            //Act.
            IActionResult result = controller.UnFollowTeam(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;

            //Assert.
            service.Verify(us => us.UnFollowTeam(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void UnfollowTeamNoDataAccessTest()
        {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            Exception toThrow = new DataInaccessibleException();
             service.Setup(us => us.UnFollowTeam(It.IsAny<string>(), It.IsAny<int>())).Throws(toThrow);
             TeamModelIn input = GetTeamModelIn();

            //Act.
            IActionResult result = controller.UnFollowTeam(input);
            ObjectResult notData = result as ObjectResult;
            ErrorModelOut error = notData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(notData);
            Assert.AreEqual(500,notData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);
        }

        [TestMethod]
        public void GetFollowedTeamsTest() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            Team aTeam = new Team("aTeam", "aPhoto", new Sport("aSport"));
            ICollection<Team> list2return = new List<Team>() { aTeam, aTeam, aTeam };
            service.Setup(us => us.GetUserTeams(It.IsAny<string>())).Returns(list2return);

            //Act.
            IActionResult result = controller.GetFollowedTeams("username");
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<TeamModelOut> teamsFollowed = okResult.Value as ICollection<TeamModelOut>;

            //Assert.
            service.Verify(us => us.GetUserTeams("username"), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(teamsFollowed);
            Assert.AreEqual(list2return.Count,teamsFollowed.Count);
        }

        [TestMethod]
        public void GetFollowedTeamsNotExistingUserTest() {
            //Arrange.
            Exception toThrow = new UserNotFoundException();
            service.Setup(us => us.GetUserTeams(It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.GetFollowedTeams("username");
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert.
            service.Verify(us => us.GetUserTeams("username"), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);

        }

        [TestMethod]
        public void GetFollowedTeamsNoDataAccessTest()
        {
             //Arrange.
             Exception toThrow = new DataInaccessibleException();
            service.Setup(us => us.GetUserTeams(It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.GetFollowedTeams("username");
            ObjectResult notData = result as ObjectResult;
            ErrorModelOut error = notData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(notData);
            Assert.AreEqual(500,notData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message,error.ErrorMessage);
           
        }

        private TeamModelIn GetTeamModelIn()
        {
            TeamModelIn fake = new TeamModelIn() { Name = "Internazionale de Milano", SportName = "Soccer", Id = 3, Photo="" };
            return fake;
        }
        private ControllerContext GetFakeControllerContext() {
            ICollection<Claim> fakeClaims = new List<Claim>() { new Claim("Username", "username") };

            Mock<ClaimsPrincipal> cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.Claims).Returns(fakeClaims);
            Mock<HttpContext> contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(cp.Object);

            Mock<ControllerContext> controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Object.HttpContext = contextMock.Object;
            return controllerContextMock.Object;
        }
        private User GetFakeUser()
        {
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