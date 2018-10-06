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
        ICollection<Team> teamsCollection;
        ICollection<Match> matchesCollection;
        IFixtureService fixtureService;

        [TestInitialize]
        public void Initialize()
        {
            SetUpController();
            SetUpData();
            ConfigureFixtureService();
        }

        private void ConfigureFixtureService()
        {
            fixtureService = new Mock<IFixtureService>().Object;
            Mock.Get(fixtureService).Setup(f => f.AddFixture(teamsCollection)).Returns(matchesCollection);
        }

        private void SetUpData()
        {
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

            testSport = new Sport("Football");

            sportsRepo.Setup(r => r.Get((testSport.Name))).Returns(testSport);
            sportsRepo.Setup(r => r.Get(It.Is<String>(x => (x != testSport.Name)))).Throws(new SportNotFoundException());
            sportsRepo.Setup(r => r.GetAll()).Returns(new List<Sport>() { testSport });

            controller = new SportsController(sportsRepo.Object);
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
            IActionResult result = controller.CreateFixture(input);
            CreatedResult createdResult = result as CreatedResult;
            ICollection<MatchModelOut> modelOut = createdResult.Value as ICollection<MatchModelOut>;

            //Assert
            Mock.Get(fixtureService).Verify(s => s.AddFixture(teamsCollection), Times.Once);
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
            Exception toThrow = new WrongFixtureException();
            Mock.Get(fixtureService).Setup(s => s.AddFixture(It.IsAny<ICollection<Team>>())).Throws(toThrow);


            //Act
            IActionResult result = controller.CreateFixture(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert
            Mock.Get(fixtureService).Verify(s => s.AddFixture(teamsCollection), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void CreateFixtureTeamNotFoundTest()
        {
            //Arrange
            FixtureModelIn input = new FixtureModelIn()
            {
                Day = DateTime.Now.Day,
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year
            };
            Exception toThrow = new TeamNotFoundException();
            Mock.Get(fixtureService).Setup(s => s.AddFixture(It.IsAny<ICollection<Team>>())).Throws(toThrow);


            //Act
            IActionResult result = controller.CreateFixture(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert
            Mock.Get(fixtureService).Verify(s => s.AddFixture(teamsCollection), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }
    }
}
