using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.DataAccess;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Match = ObligatorioDA2.BusinessLogic.Match;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using System.IO;
using ObligatorioDA2.BusinessLogic.FixtureAlgorithms;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class FixtureServiceTest
    {
        private IFixtureGenerator oneMatchGenerator;
        private IFixtureGenerator twoMatchsGenerator;

        private Sport sport;
        private Team teamA;
        private Team teamB;
        private Team teamC;
        private Team teamD;
        private ICollection<Team> teamsCollection;
        private DateTime initialDate;
        private IMatchRepository matchStorage;
        private ITeamRepository teamStorage;
        private ISportRepository sportStorage;
        private FixtureService fixtureService;

        [TestInitialize]
        public void Initialize()
        {
            sport = new Mock<Sport>("Soccer",true).Object;
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
            oneMatchGenerator = new OneMatchFixture(initialDate, 2, 5);
            twoMatchsGenerator = new HomeAwayFixture(initialDate, 2, 5);
            SetUpRepository();
            matchStorage.Clear();
            teamStorage.Clear();
            fixtureService = new FixtureService(matchStorage, teamStorage, sportStorage);
        }

        private void SetUpRepository()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "FixtureService")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            matchStorage = new MatchRepository(context);
            teamStorage = new TeamRepository(context);
            sportStorage = new SportRepository(context);
        }

        private void AddTeamsToRepo()
        {
            foreach (Team team in teamsCollection)
            {
                teamStorage.Add(team);
            }
        }

        [TestMethod]
        public void SetFixtureAlgorithmTest()
        {
            fixtureService.FixtureAlgorithm = twoMatchsGenerator;
            Assert.AreEqual(fixtureService.FixtureAlgorithm, twoMatchsGenerator);
        }

        [TestMethod]
        public void AddFixtureOneMatchTest()
        {
            fixtureService.FixtureAlgorithm = oneMatchGenerator;
            ICollection<Match> matchesAdded = fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.Id)));
        }

        [TestMethod]
        public void AddFixtureTwoMatchesTest()
        {
            fixtureService.FixtureAlgorithm = twoMatchsGenerator;
            ICollection<Match> matchesAdded = fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.Id)));
        }

        [TestMethod]
        public void AddFixtureOneMatchImpairTest()
        {
            teamsCollection.Remove(teamD);
            fixtureService.FixtureAlgorithm = oneMatchGenerator;
            ICollection<Match> matchesAdded = fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.Id)));
        }

        [TestMethod]
        public void AddFixtureTwoMatchesImpairTest()
        {
            teamsCollection.Remove(teamD);
            fixtureService.FixtureAlgorithm = twoMatchsGenerator;
            ICollection<Match> matchesAdded = fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.Id)));
        }

        [TestMethod]
        public void AddFixtureWithNamesTest()
        {
            AddTeamsToRepo();
            fixtureService.FixtureAlgorithm = new OneMatchFixture(DateTime.Now, 2, 5);
            ICollection<string> teamsNames = teamsCollection.Select(t => t.Name).ToList();
            string sportName = sport.Name;
            ICollection<Match> matchesAdded = fixtureService.AddFixture(teamsNames, sportName);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.Id)));
        }

        [TestMethod]
        public void AddFixtureOfSport()
        {
            AddTeamsToRepo();
            fixtureService.FixtureAlgorithm= new OneMatchFixture(DateTime.Now, 2, 5);
            ICollection<Match> matchesAdded = fixtureService.AddFixture(sport);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.Id)));
        }

        //Exceptions

        [TestMethod]
        [ExpectedException(typeof(WrongFixtureException))]
        public void AddFixtureTeamAlreadyHasMatchTest()
        {
            fixtureService.FixtureAlgorithm = new OneMatchFixture(DateTime.Now, 2, 5);
            Match aMatch = new Match(teamA, teamB, initialDate, sport);
            matchStorage.Add(aMatch);
            fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchStorage.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void AddFixtureTeamNotFoundTest()
        {
            ICollection<string> teamsNames = teamsCollection.Select(t => t.Name).ToList();
            string sportName = sport.Name;
            ICollection<Match> matchesAdded = fixtureService.AddFixture(teamsNames, sportName);
        }

        [TestMethod]
        public void GetAvailableStrategiesTest() {
            string path = "C://Users//Marcel//Desktop//diseño2//Arancet_Cohen//ObligatorioDA2//Backend//ObligatorioDA2.BusinessLogic//bin//Debug//BusinessLogic.dll";
            ICollection<Type> algorithms = fixtureService.GetAlgorithms(path);
            Assert.AreEqual(0, algorithms.Count);
        }
    }
}
