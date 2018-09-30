using System;
using System.Collections.Generic;
using BusinessLogic;
using DataAccess;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using Match = BusinessLogic.Match;
using DataRepositories;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class MatchServiceTest
    {
        private MatchService serviceToTest;
        //Mock<IMatchRepository> repoDouble;
        private IMatchRepository repoDouble;
        private ITeamRepository teamsRepo;
        private Sport sport;
        private Team teamA;
        private Team teamB;
        private Team teamC;
        private Match matchAvsB;
        private Match matchAvsC;
        private Match matchBvsC;
        private List<Match> storedMatches;

        [TestInitialize]
        public void SetUp() {
            /*repoDouble = new Mock<IMatchRepository>();
            fakeMatch = BuildMatch();
            serviceToTest = new MatchService(repoDouble.Object);
            repoDouble.Setup(r => r.Get(2)).Returns(fakeMatch.Object);
            repoDouble.Setup(r => r.Get(It.Is<int>(i => i != 2))).Throws(new MatchNotFoundException());
            storedMatches = new List<Match>() { fakeMatch.Object };
            repoDouble.Setup(r => r.GetAll()).Returns(storedMatches);*/
            sport = new Mock<Sport>("Soccer").Object;
            teamA = new Mock<Team>(1, "teamA", "photo", sport).Object;
            teamB = new Mock<Team>(2, "teamB", "photo", sport).Object;
            teamC = new Mock<Team>(3, "teamC", "photo", sport).Object;
            matchAvsB = new Mock<Match>(1, teamA, teamB, DateTime.Now.AddDays(1), sport).Object;
            matchAvsC = new Mock<Match>(2, teamA, teamC, DateTime.Now.AddDays(2), sport).Object;
            matchBvsC = new Mock<Match>(3, teamA, teamB, DateTime.Now.AddDays(3), sport).Object;
            SetUpRepository();
            serviceToTest = new MatchService(repoDouble, teamsRepo);
            //repoDouble.Setup(r => r.Add(It.Is<Match>(i => i is Match)))
        }

        private void SetUpRepository() {

            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
               .UseInMemoryDatabase(databaseName: "MatchRepository")
               .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            repoDouble = new MatchRepository(context);
            teamsRepo = new TeamRepository(context);
        }

        private Match BuildMatch(int id, string homeTeamName, string awayTeamName, string sportName)
        {
            Mock<Team> home = new Mock<Team>("Manchester United", "aPath");
            Mock<Team> away = new Mock<Team>("Real Madrid", "aPath");
            Mock<Sport> sport = new Mock<Sport>("Soccer");
            Mock<Match> match = new Mock<Match>(3, home.Object, away.Object, DateTime.Now, sport.Object);
            return match.Object;
        }

        [TestMethod]
        public void AddMatchTest() {
            serviceToTest.AddMatch(matchAvsB);
            Assert.AreEqual(serviceToTest.GetAllMatches().Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchAlreadyExistsException))]
        public void AddAlreadyExistentTesT() {
            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.AddMatch(matchAvsB);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void GetUnexistentMatchTest() {
            serviceToTest.GetMatch(9);
        }

        [TestMethod]
        public void GetExistentMatchTest() {
            serviceToTest.AddMatch(matchAvsB);
            Match retrieved = serviceToTest.GetMatch(matchAvsB.Id);
            Assert.AreEqual(retrieved.Id, matchAvsB.Id);

        }

        [TestMethod]
        public void GetMatchesOfSportTest() {
            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.AddMatch(matchBvsC);
            ICollection<Match> matches = serviceToTest.GetAllMatches(sport);
            Assert.AreEqual(matches.Count, 2);
          
        }


        [TestMethod]
        public void GetMatchesFromTeamTest() {
            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.AddMatch(matchAvsC);
            serviceToTest.AddMatch(matchBvsC);
            ICollection<Match> matches = serviceToTest.GetAllMatches(teamA);
            Assert.AreEqual(matches.Count, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetMatchesFromTeamNotExistentTest()
        {
            serviceToTest.GetAllMatches(teamA);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void DeleteMatchTest() {
            serviceToTest.DeleteMatch(3);
        }

        [TestMethod]
        public void ModifyTest() {
            serviceToTest.AddMatch(matchAvsC);
            Match modifiedAvsB = new Mock<Match>(1, teamB, teamA, matchAvsB.Date.AddDays(1), sport).Object;
            serviceToTest.ModifyMatch(modifiedAvsB);
            Match modified = serviceToTest.GetMatch(matchAvsC.Id);

            Assert.AreEqual(modifiedAvsB.HomeTeam.Name, modified.HomeTeam.Name);
            Assert.AreEqual(modifiedAvsB.AwayTeam.Name, modified.AwayTeam.Name);
            Assert.AreEqual(modifiedAvsB.Date, modified.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void ModifyUnexistentTest() {
            serviceToTest.ModifyMatch(matchAvsC);
        }

        [TestMethod]
        public void GetAllMatchesTest() {
            serviceToTest.AddMatch(matchAvsC);
            serviceToTest.AddMatch(matchAvsB);
            Assert.AreEqual(serviceToTest.GetAllMatches().Count, 2);
        }

        [TestMethod]
        public void ExistMatchTrueTest()
        {
            serviceToTest.AddMatch(matchAvsB);
            Assert.IsTrue(serviceToTest.Exists(matchAvsB));
        }

        [TestMethod]
        public void ExistMatchFalseTest()
        {
            serviceToTest.AddMatch(matchAvsB);
            Assert.IsFalse(serviceToTest.Exists(matchAvsC));
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyHasMatchException))]
        public void TeamWithTwoMatchesSameDateTest()
        {
            matchAvsC.Date = matchAvsB.Date;
            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.AddMatch(matchAvsC);
        }



    }
}
