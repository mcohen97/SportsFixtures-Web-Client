using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using DataAccess;
using DataRepositories;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.WebAPI.Controllers;
using ObligatorioDA2.WebAPI.Models;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class SportsControllerTest
    {
        private Mock<ISportRepository> sportsRepo;
        private Mock<ITeamRepository> teamsRepo;
        private SportsController controllerToTest;

        [TestInitialize]
        public void SetUp() {
            SetUpRepository();
        }

        private void SetUpRepository()
        {
            sportsRepo = new Mock<ISportRepository>();
            teamsRepo = new Mock<ITeamRepository>();
            Mock<IMatchRepository> matchesRepo = new Mock<IMatchRepository>();

            Sport testSport1 = new Sport("Tennis");
            Sport testSport2 = new Sport("Basketball");

            sportsRepo.Setup(r => r.Get("Tennis")).Returns(testSport1);
            sportsRepo.Setup(r => r.Get(It.Is<String>(x => (x != "Tennis") && (x !="Basketball")))).Throws(new SportNotFoundException());
            sportsRepo.Setup(r => r.GetAll()).Returns(new List<Sport>() {new Sport("Basketball"), new Sport("Tennis") });

            controllerToTest = new SportsController(sportsRepo.Object,matchesRepo.Object,teamsRepo.Object);
        }

        [TestMethod]
        public void CreateSportTest() {
            SportModelIn input= new SportModelIn()
            {
                Name = "Soccer"
            };

            IActionResult result = controllerToTest.Post(input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            SportModelOut output = createdResult.Value as SportModelOut;

            sportsRepo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(createdResult.StatusCode, 201);
            Assert.AreEqual(createdResult.RouteName, "GetSportById");
            Assert.IsNotNull(output);
            Assert.AreEqual(output.Name, input.Name);
        }

        [TestMethod]
        public void CreateSportAlreadyExistingTest() {
            //Arrange.
            Exception toThrow = new SportAlreadyExistsException();
            sportsRepo.Setup(r => r.Add(It.IsAny<Sport>())).Throws(toThrow);
            SportModelIn input = new SportModelIn()
            {
                Name = "Soccer"
            };

            //Act.
            IActionResult result = controllerToTest.Post(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert.
            sportsRepo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400,badRequest.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void CreateInvalidSportTest() {
            SportModelIn input = new SportModelIn();

            controllerToTest.ModelState.AddModelError("", "Error");
            IActionResult result = controllerToTest.Post(input);

            BadRequestObjectResult createdResult = result as BadRequestObjectResult;

            sportsRepo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Never);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void GetSportTest() {

            IActionResult result = controllerToTest.Get("Tennis");
            OkObjectResult okResult = result as OkObjectResult;
            SportModelOut modelOut = okResult.Value as SportModelOut;

            sportsRepo.Verify(r => r.Get("Tennis"), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(modelOut.Name, "Tennis");
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void GetNotExistentTest() {
            IActionResult result = controllerToTest.Get("Golf");
            NotFoundObjectResult notFound = result as NotFoundObjectResult;

            sportsRepo.Verify(r => r.Get("Golf"), Times.Once);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(notFound.StatusCode, 404);
        }

        [TestMethod]
        public void GetAlltest() {
            IActionResult result = controllerToTest.Get();

            OkObjectResult okResult = result as OkObjectResult;
            IEnumerable<SportModelOut> resultObject = okResult.Value as IEnumerable<SportModelOut>;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(resultObject);
        }

        [TestMethod]
        public void DeleteTest() {
            IActionResult result = controllerToTest.Delete("Tennis");

            OkResult okResult = result as OkResult;

            sportsRepo.Verify(r => r.Delete("Tennis"), Times.Once);
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public void DeleteNotFoundTest() {
            sportsRepo.Setup(r => r.Delete("Golf")).Throws(new SportNotFoundException());

            IActionResult result = controllerToTest.Delete("Golf");
            NotFoundObjectResult notFound = result as NotFoundObjectResult;

            sportsRepo.Verify(r => r.Delete("Golf"), Times.Once);
            Assert.IsNotNull(notFound);
            Assert.IsNotNull(notFound.Value);
        }

        [TestMethod]
        public void PutModifyTest() {

            SportModelIn input = new SportModelIn() { Name = "Soccer" };
            IActionResult result = controllerToTest.Put("Soccer",input);

            OkObjectResult okResult = result as OkObjectResult;

            sportsRepo.Verify(r => r.Modify(It.IsAny<Sport>()), Times.Once);
            sportsRepo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Never);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
        }

        [TestMethod]
        public void PutAddTest() {
            sportsRepo.Setup(r => r.Modify(It.IsAny<Sport>())).Throws(new SportNotFoundException());
            SportModelIn input = new SportModelIn() { Name = "Soccer" };
            IActionResult result = controllerToTest.Put("Soccer",input);

            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;

            sportsRepo.Verify(r => r.Modify(It.IsAny<Sport>()), Times.Once);
            sportsRepo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Once);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(createdResult.StatusCode, 201);
            Assert.AreEqual(createdResult.RouteName, "GetSportById");
        }

        [TestMethod]
        public void PutInvalidTest() {
            SportModelIn input = new SportModelIn() {};
            controllerToTest.ModelState.AddModelError("", "Error");
            IActionResult result = controllerToTest.Put("Soccer",input);


            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void GetSportTeamsTest() {
            //Arrange.
            Team dummyTeam = new Team("Dummy", "Dummy", new Sport("Dummy"));
            ICollection<Team> cannedResponse = new List<Team>() { dummyTeam, dummyTeam, dummyTeam };
            sportsRepo.Setup(r => r.Get(It.IsAny<string>())).Returns(new Sport("Dummy"));
            teamsRepo.Setup(r => r.GetTeams(It.IsAny<string>())).Returns(cannedResponse);

            //Act.
            IActionResult result = controllerToTest.GetTeams("Dummy");
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<TeamModelOut> teams = okResult.Value as ICollection<TeamModelOut>;

            //Assert.
            teamsRepo.Verify(r => r.GetTeams(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(teams);
            Assert.AreEqual(cannedResponse.Count, teams.Count);
        }

        [TestMethod]
        public void GetSportTeamsNotFoundTest() {
            //Arrange.
            Exception toThrow = new SportNotFoundException();
            teamsRepo.Setup(r => r.GetTeams(It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controllerToTest.GetTeams("Dummy");
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            teamsRepo.Verify(r => r.GetTeams(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

    }
}
