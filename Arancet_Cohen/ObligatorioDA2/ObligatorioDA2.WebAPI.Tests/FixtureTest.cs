using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.WebAPI.Controllers;
using ObligatorioDA2.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Match = BusinessLogic.Match;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class FixtureTest
    {
        SportsController controller;
        Sport testSport;
        Mock<ISportRepository> sportsRepo;
        Mock<IMatchRepository> matchesRepo;
        Mock<ITeamRepository> teamsRepo;
        ICollection<Team> teamsCollection;
        ICollection<Match> matchesCollection;

        [TestInitialize]
        public void Initialize()
        {
            SetUpData();
            SetUpController();
        }

        private void SetUpData()
        {
            testSport = new Sport("Football");

            Team teamA = new Team(1, "teamA", "photo", testSport);
            Team teamB = new Team(2, "teamB", "photo", testSport);
            Team teamC = new Team(3, "teamC", "photo", testSport);
            Team teamD = new Team(4, "teamD", "photo", testSport);
            teamsCollection = new List<Team>
            {
                teamA,
                teamB,
                teamC,
                teamD
            };
            matchesCollection = new List<Match>
            {
                new Match(1, teamA, teamB, DateTime.Now.AddDays(1), testSport),
                new Match(2, teamC, teamD, DateTime.Now.AddDays(1), testSport),
                new Match(3, teamA, teamC, DateTime.Now.AddDays(2), testSport),
                new Match(4, teamD, teamB, DateTime.Now.AddDays(2), testSport),
                new Match(5, teamA, teamD, DateTime.Now.AddDays(3), testSport),
                new Match(6, teamC, teamB, DateTime.Now.AddDays(4), testSport)
            };
        }

        private void SetUpController()
        {
            sportsRepo = new Mock<ISportRepository>();
            sportsRepo.Setup(r => r.Get((testSport.Name))).Returns(testSport);
            sportsRepo.Setup(r => r.Get(It.Is<String>(x => (x != testSport.Name)))).Throws(new SportNotFoundException());
            sportsRepo.Setup(r => r.GetAll()).Returns(new List<Sport>() { testSport });

            matchesRepo = new Mock<IMatchRepository>();
            matchesRepo.Setup(m => m.Add(It.IsAny<Match>())).Returns((Match mat) => { return mat; });
            matchesRepo.Setup(m => m.Exists(It.IsAny<Match>())).Returns(false);
            matchesRepo.Setup(m => m.GetAll()).Returns(new List<Match>());

            teamsRepo = new Mock<ITeamRepository>();
            teamsRepo.Setup(t => t.GetTeams(It.IsAny<string>())).Returns(teamsCollection);
            teamsRepo.Setup(t => t.GetAll()).Returns(teamsCollection);

            controller = new SportsController(sportsRepo.Object, matchesRepo.Object, teamsRepo.Object);
        }

        [TestMethod]
        public void CreateFixtureTest()
        {
            //Arrange
            FixtureModelIn input = new FixtureModelIn()
            {
                Day = DateTime.Now.Day,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year
            };

            //Act
            IActionResult result = controller.CreateOneMatchFixture(testSport.Name,input);
            CreatedResult createdResult = result as CreatedResult;
            ICollection<MatchModelOut> modelOut = createdResult.Value as ICollection<MatchModelOut>;

            //Assert
            //Mock.Get(fixtureService).Verify(s => s.AddFixture(testSport), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(modelOut.Count, matchesCollection.Count);
        }

        [TestMethod]
        public void CreateFixtureWrongFixtureTest()
        {
            //Arrange
            FixtureModelIn input = new FixtureModelIn()
            {
                Day = DateTime.Now.Day,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year
            };
            matchesRepo.Setup(m => m.GetAll()).Returns(matchesCollection);
            ICollection<string> errorMessagges = TeamAlreadyHasMatchErrorMessagges(matchesCollection);


            //Act
            IActionResult result = controller.CreateOneMatchFixture(testSport.Name, input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.IsTrue(errorMessagges.Contains(error.ErrorMessage));
        }

        private ICollection<string> TeamAlreadyHasMatchErrorMessagges(ICollection<Match> matches)
        {
            ICollection<string> errorMessagges = new List<string>();
            foreach (Match aMatch in matches)
            {
                errorMessagges.Add(aMatch.HomeTeam.Name + " already has a match on date " + new DateTime(aMatch.Date.Year,aMatch.Date.Month,aMatch.Date.Day));
                errorMessagges.Add(aMatch.AwayTeam.Name + " already has a match on date " + new DateTime(aMatch.Date.Year, aMatch.Date.Month, aMatch.Date.Day));
            }
            return errorMessagges;
        }

        [TestMethod]
        public void CreateFixtureBadModelTest()
        {
            //Arrange
            FixtureModelIn input = new FixtureModelIn()
            {

            };
            controller.ModelState.AddModelError("", "Error");

            //Act
            IActionResult result = controller.CreateOneMatchFixture(testSport.Name, input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

    }
}
