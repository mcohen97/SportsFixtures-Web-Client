using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class MatchesControllerTest{

        private Mock<IMatchService> matchService;
        private MatchesController controller;
        private Match testMatch;

        [TestInitialize]
        public void SetUp() {
            matchService = new Mock<IMatchService>();
            testMatch = BuildFakeMatch();
            controller = new MatchesController(matchService.Object);
        }

        private Match BuildFakeMatch()
        {
            Sport played = new Sport("Football");
            Team home = new Team("Patriots", "aPhoto", played);
            Team away = new Team("Falcons", "aPhoto", played);
            Match built = new Match(home, away, DateTime.Now, played);
            return built;
        }

        [TestMethod]
        public void GetTest()
        {
            //Arrange.
            matchService.Setup(s => s.GetAllMatches()).Returns(new List<Match>() { testMatch });

            //Act.
            IActionResult result = controller.Get();
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<MatchModelOut> matches = okResult.Value as ICollection<MatchModelOut>;

            //Assert.
            matchService.Verify(s => s.GetAllMatches(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(matches);
            Assert.AreEqual(matches.Count, 1);
        }

        [TestMethod]
        public void CreateValidMatchTest() {
            matchService.Setup(ms => ms.AddMatch(testMatch)).Returns(testMatch);
            MatchModelIn input = BuildMatchModelIn(testMatch);

            IActionResult result = controller.Post(input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            MatchModelOut created = createdResult.Value as MatchModelOut;

            matchService.Verify(ms => ms.AddMatch(testMatch), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual("GetMatchById", createdResult.RouteName);
            Assert.IsNotNull(created);
            Assert.AreEqual(testMatch.Date, testMatch.Date);
        }

        private MatchModelIn BuildMatchModelIn(Match aMatch)
        {
            MatchModelIn built = new MatchModelIn()
            {
                SportName = aMatch.Sport.Name,
                AwayTeamId = aMatch.AwayTeam.Id,
                HomeTeamId = aMatch.HomeTeam.Id,
                Date = aMatch.Date
            };
            return built;
        }

        [TestMethod]
        public void CreateInvalidMatchTest() {
            //Arrange.
            MatchModelIn input = new MatchModelIn() { };
            //We need to force the error in de ModelState.
            controller.ModelState.AddModelError("", "Error");

            //Act.
            IActionResult result = controller.Post(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;

            matchService.Verify(ms => ms.AddMatch(testMatch), Times.Never);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void CreateMatchSameDayForTeamTest() {
            //Arrange.
            matchService.Setup(ms => ms.AddMatch(testMatch)).Throws(new TeamAlreadyHasMatchException());
            MatchModelIn input = BuildMatchModelIn(testMatch);

            //Act.
            IActionResult result = controller.Post(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;


            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.IsNotNull(error);
        }

    }
   
}
