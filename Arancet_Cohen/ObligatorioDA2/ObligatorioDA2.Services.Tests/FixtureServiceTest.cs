using BusinessLogic;
using DataAccess;
using DataRepositories;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Match = BusinessLogic.Match;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class FixtureServiceTest
    {
        private FixtureGenerator oneMatchGenerator;
        private FixtureGenerator twoMatchsGenerator;

        private Sport sport;
        private Team teamA;
        private Team teamB;
        private Team teamC;
        private Team teamD;
        private ICollection<Team> teamsCollection;
        private DateTime initialDate;
        private IMatchRepository matchesStorage;
        private FixtureService fixtureService;

        [TestInitialize]
        public void Initialize()
        {
            sport = new Mock<Sport>("Soccer").Object;
            teamA = new Mock<Team>(1, "teamA", "photo", sport).Object;
            teamB = new Mock<Team>(2, "teamB", "photo", sport).Object;
            teamC = new Mock<Team>(3, "teamC", "photo", sport).Object;
            teamD = new Mock<Team>(4, "teamD", "photo", sport).Object;
            teamsCollection = new List<Team> {
                teamA,
                teamB,
                teamC,
                teamD
            };

            initialDate = DateTime.Now;
            oneMatchGenerator = new OneMatchFixture(initialDate, 2, 5, sport);
            twoMatchsGenerator = new HomeAwayFixture(initialDate, 2, 5, sport);
            SetUpRepository();
            fixtureService = new FixtureService();
        }

        private void SetUpRepository()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "MatchRepository")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            matchesStorage = new MatchRepository(context);
        }

        [TestMethod]
        public void SetFixtureAlgorithmTest()
        {
            fixtureService.FixtureAlgorithm = twoMatchsGenerator;
            Assert.AreEqual(fixtureService.FixtureAlgorithm, twoMatchsGenerator);
        }

        [TestMethod]
        public void AddMatchesTest()
        {
            fixtureService.FixtureAlgorithm = oneMatchGenerator;
            ICollection<Match> matchesGenerated = oneMatchGenerator.GenerateFixture(teamsCollection);
            fixtureService.AddMatches(teamsCollection);
            Assert.IsTrue(matchesGenerated.All(m => matchesStorage.Exists(m.Id)));
        }



    }
}
