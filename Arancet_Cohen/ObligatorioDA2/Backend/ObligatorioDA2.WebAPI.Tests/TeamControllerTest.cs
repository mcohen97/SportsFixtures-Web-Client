using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Data.Repositories;
using System;
using System.Collections.Generic;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class TeamControllerTest
    {
        private TeamsController controller;
        private Mock<ITeamRepository> teamsRepo;
        private Mock<ISportRepository> sportsRepo;
        Team team;

        [TestInitialize]
        public void SetUp() {
            Sport testSport = new Sport("Soccer", true);
            team = new Team(2,"Nacional", "/MyResource/Nacional.png",testSport);
            teamsRepo = new Mock<ITeamRepository>();
            sportsRepo = new Mock<ISportRepository>();
            sportsRepo.Setup(r => r.Get("Soccer")).Returns(testSport);
            controller = new TeamsController(teamsRepo.Object,sportsRepo.Object);
        }

        [TestMethod]
        public void GetAllTeamsTest() {
            //Arrange.
            ICollection<Team> dummyTeams = new List<Team>() { team, team, team };
            teamsRepo.Setup(r => r.GetAll()).Returns(dummyTeams);

            //Act.
            IActionResult result = controller.Get();
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<TeamModelOut> allTeams = okResult.Value as ICollection<TeamModelOut>;

            //Assert.
            teamsRepo.Verify(r => r.GetAll(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(allTeams);
            Assert.AreEqual(dummyTeams.Count,allTeams.Count);
        }

        [TestMethod]
        public void GetAllNoDataAccessTest()
        {

            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            teamsRepo.Setup(us => us.GetAll()).Throws(toThrow);


            //Act.
            IActionResult result = controller.Get();
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
        public void GetTeamByNamesTest()
        {

            //Arrange.
            teamsRepo.Setup(r => r.Get(team.Sport.Name, team.Name)).Returns(team);

            //Act.
            IActionResult result = controller.Get("Soccer", "Nacional");
            OkObjectResult okResult = result as OkObjectResult;
            TeamModelOut resultTeam = okResult.Value as TeamModelOut;

            //Assert.
            teamsRepo.Verify(r => r.Get("Soccer", "Nacional"), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(resultTeam.Name, team.Name);
        }

        [TestMethod]
        public void GetTeamByNamesNotFoundTest()
        {
            //Arrange.
            Exception toThrow = new TeamNotFoundException();
            teamsRepo.Setup(r => r.Get(It.IsAny<string>(), It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Get("Basketball", "DreamTeam");
            NotFoundObjectResult notFoundResult = result as NotFoundObjectResult;
            ErrorModelOut error = notFoundResult.Value as ErrorModelOut;

            //Assert.
            teamsRepo.Verify(r => r.Get("Basketball", "DreamTeam"), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void GetTeamByNameNoDataAccessTest()
        {

            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            teamsRepo.Setup(r => r.Get(It.IsAny<string>(), It.IsAny<string>())).Throws(toThrow);


            //Act.
            IActionResult result = controller.Get("Basketball", "DreamTeam");
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
        public void GetTeamByIdTest() {
            //Arrange.
            teamsRepo.Setup(r => r.Get(2)).Returns(team);

            //Act.
            IActionResult result = controller.Get(2);
            OkObjectResult okResult = result as OkObjectResult;
            TeamModelOut resultTeam = okResult.Value as TeamModelOut;

            //Assert.
            teamsRepo.Verify(r => r.Get(2), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(resultTeam.Name, team.Name);
        }

        [TestMethod]
        public void GetTeamByIdNotFoundTest() {
            //Arrange.
            Exception toThrow = new TeamNotFoundException();
            teamsRepo.Setup(r => r.Get(It.IsAny<int>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Get(2);
            NotFoundObjectResult notFoundResult = result as NotFoundObjectResult;
            ErrorModelOut error = notFoundResult.Value as ErrorModelOut;

            //Assert.
            teamsRepo.Verify(r => r.Get(2), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void GetTeamByIdNoDataAccessTest()
        {
            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            teamsRepo.Setup(r => r.Get(It.IsAny<int>())).Throws(toThrow);


            //Act.
            IActionResult result = controller.Get(2);
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
        public void CreateValidTeamTest()
        {
            //Arrange.
            TeamModelIn input = CreateTeamModelIn();
            teamsRepo.Setup(r => r.Add(It.IsAny<Team>())).Returns(team);

            //Act.
            IActionResult result = controller.Post(input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            TeamModelOut modelOut = createdResult.Value as TeamModelOut;

            //Assert
            teamsRepo.Verify(r => r.Add(It.IsAny<Team>()), Times.Once);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetTeamById", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(input.Name, modelOut.Name);
        }

        private TeamModelIn CreateTeamModelIn()
        {
            return new TeamModelIn()
            {
                Name = team.Name,
                Photo = team.Photo,
                SportName = team.Sport.Name
            };
        }

        [TestMethod]
        public void CreateFailedTeamRequiredFieldsTest()
        {
            //Arrange.
           TeamModelIn modelIn = new TeamModelIn() {
                 Photo = "/MyResource/DreamTeam.png"
            };
            //We need to force the error in de ModelState.
            controller.ModelState.AddModelError("", "Error");

            //Act.
            IActionResult result = controller.Post(modelIn);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            
            //Assert.
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void CreateAlreadyExistingTeam() {
            //Arrange.
            Exception toThrow = new TeamAlreadyExistsException();
            teamsRepo.Setup(tr => tr.Add(It.IsAny<Team>())).Throws(toThrow);
            TeamModelIn input = CreateTeamModelIn();

            //Act.
            IActionResult result = controller.Post(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void CreateNoDataAccessTest()
        {
            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            teamsRepo.Setup(tr => tr.Add(It.IsAny<Team>())).Throws(toThrow);
            TeamModelIn input = CreateTeamModelIn();

            //Act.
            IActionResult result = controller.Post(input);
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
        public void PutTest() {
            //Arrange.
            TeamModelIn input = CreateTeamModelIn();

            //Act.
            IActionResult result = controller.Put(2,input);
            OkObjectResult okResult = result as OkObjectResult;
            TeamModelOut modified = okResult.Value as TeamModelOut;

            //Assert.
            teamsRepo.Verify(r => r.Modify(It.IsAny<Team>()), Times.Once);
            teamsRepo.Verify(r => r.Add(It.IsAny<Team>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200,okResult.StatusCode);
        }

        [TestMethod]
        public void PutAddTest() {

            //Arrange.
            teamsRepo.Setup(r => r.Modify(It.IsAny<Team>())).Throws(new TeamNotFoundException());
            TeamModelIn input = CreateTeamModelIn();

            //Act.
            IActionResult result =controller.Put(2,input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            TeamModelOut modelOut = createdResult.Value as TeamModelOut;

            //Assert.
            teamsRepo.Verify(r => r.Modify(It.IsAny<Team>()), Times.Once);
            teamsRepo.Verify(r => r.Add(It.IsAny<Team>()), Times.Once);
            Assert.AreEqual("GetTeamById", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(2, modelOut.Id);
        }

        [TestMethod]
        public void PutWrongFormatTest() {
            //Arrange.
            TeamModelIn modelIn = new TeamModelIn(){Photo = "/MyResource/DreamTeam.png"};
            //We need to force the error in de ModelState.
            controller.ModelState.AddModelError("", "Error");

            //Act.
            IActionResult result = controller.Put(2,modelIn);
            BadRequestObjectResult createdResult = result as BadRequestObjectResult;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void PutNoDataAccessTest()
        {
            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            teamsRepo.Setup(r => r.Modify(It.IsAny<Team>())).Throws(toThrow);
            TeamModelIn input = CreateTeamModelIn();

            //Act.
            IActionResult result = controller.Put(2,input);
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
        public void DeleteByIdTest()
        {

            //Act.
            IActionResult result = controller.Delete(2);
            OkObjectResult okResult = result as OkObjectResult;
            OkModelOut okMessage = okResult.Value as OkModelOut;

            //Assert.
            teamsRepo.Verify(r => r.Delete(2), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsNotNull(okMessage);
        }

        [TestMethod]
        public void DeleteByIdNotExistentTest()
        {
            //Arrange.
            Exception toThrow = new TeamNotFoundException();
            teamsRepo.Setup(r => r.Delete(It.IsAny<int>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Delete(2);
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;
            
            //Assert.
            teamsRepo.Verify(r => r.Delete(2), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void DeleteByIdNoDataAccessTest()
        {
            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            teamsRepo.Setup(r => r.Delete(It.IsAny<int>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Delete(2);
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
        public void DeleteTest() {

            //Act.
            IActionResult result =controller.Delete("Soccer","Nacional");
            OkObjectResult okResult = result as OkObjectResult;
            OkModelOut okMessage = okResult.Value as OkModelOut;

            //Assert.
            teamsRepo.Verify(r => r.Delete("Soccer", "Nacional"), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void DeleteNotExistentTest() {
            teamsRepo.Setup(r => r.Delete(It.IsAny<string>(),It.IsAny<string>())).Throws(new TeamNotFoundException());

            IActionResult result = controller.Delete("Soccer", "Nacional");
            BadRequestObjectResult okResult = result as BadRequestObjectResult;

            teamsRepo.Verify(r => r.Delete("Soccer", "Nacional"), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(okResult.StatusCode, 400);
        }

        [TestMethod]
        public void DeleteNoDataAccessTest()
        {
            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            teamsRepo.Setup(r => r.Delete(It.IsAny<string>(), It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Delete("Soccer", "Nacional");
            ObjectResult noData = result as ObjectResult;
            ErrorModelOut error = noData.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(noData);
            Assert.AreEqual(500, noData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }
    }
}