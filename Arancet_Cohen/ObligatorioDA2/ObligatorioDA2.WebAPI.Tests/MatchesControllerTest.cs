using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            IActionResult result =controller.Get();
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<MatchModelOut> matches = okResult.Value as ICollection<MatchModelOut>;

            //Check.
            matchService.Verify(s => s.GetAllMatches(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(matches);
            Assert.AreEqual(matches.Count, 1);
        }
    }
   
}
