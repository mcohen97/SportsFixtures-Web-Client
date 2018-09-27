using System;
using System.Collections.Generic;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using Match = BusinessLogic.Match;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class MatchServiceTest
    {
        private MatchService serviceToTest;
        Mock<IMatchRepository> repoDouble;
        Mock<Match> fakeMatch;
        private List<Match> storedMatches;
        [TestInitialize]
        public void SetUp() {
            repoDouble = new Mock<IMatchRepository>();
            fakeMatch = BuildMatch();
            serviceToTest = new MatchService(repoDouble.Object);
            repoDouble.Setup(r => r.Get(2)).Returns(fakeMatch.Object);
            repoDouble.Setup(r => r.Get(It.Is<int>(i => i != 2))).Throws(new MatchNotFoundException());
            storedMatches = new List<Match>() { fakeMatch.Object };
            repoDouble.Setup(r => r.GetAll()).Returns(storedMatches);
            //repoDouble.Setup(r => r.Add(It.Is<Match>(i => i is Match)))
        }

        private Mock<Match> BuildMatch()
        {
            Mock<Team> home = new Mock<Team>("Manchester United", "aPath");
            Mock<Team> away = new Mock<Team>("Real Madrid", "aPath");
            Mock<Match> match = new Mock<Match>(3, home.Object, away.Object, DateTime.Now);
            return match;
        }

        [TestMethod]
        public void AddMatchTest() {
            serviceToTest.AddMatch(fakeMaatch.Object);
            Assert.AreEqual(serviceToTest.GetAllMatches().Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void AddAlreadyExistentTesT() {
            serviceToTest.AddMatch(fakeMaatch.Object);
            serviceToTest.AddMatch(fakeMaatch.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void GetUnexistentMatchTest() {
            serviceToTest.GetMatch(9);

        }

        [TestMethod]
        public void GetExistentMatchTest() {
            Match retrieved = serviceToTest.GetMatch(3);
            Assert.AreEqual(retrieved.HomeTeam, BuildMatch().Object.HomeTeam);
        }

        [TestMethod]
        public void GetMatchesOfSportTest() {

        }

       /* [TestMethod]
        public void GetMatchesOfSportNotExistentTest()
        {

        }*/

        [TestMethod]
        public void GetMatchesFromTeamTest() {

        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetMatchesFromTeamNotExistentTest()
        {

        }

        [TestMethod]
        public void DeleteMatchTest() {
            serviceToTest.AddMatch(fakeMatch.Object);
            serviceToTest.DeleteMatch(3);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void DeleteNotExistentTest() {
            serviceToTest.Delete(9);
        }

        [TestMethod]
        public void ModifyTest() {
            serviceToTest.AddMatch(fakeMatch.Object);
            serviceToTest.ModifyMatch(BuildModifiedFakeMatch().Object);
            Match modified = serviceToTest.GetMatch(3);
            Assert.AreEqual(modified, BuildModifiedFakeMatch().Object);
        }

        private Mock<Match> BuildModifiedFakeMatch()
        {
            Mock<Team> home = new Mock<Team>("Manchester United", "aPath");
            Mock<Team> away = new Mock<Team>("Bayern Munich", "aPath");
            Mock<Match> match = new Mock<Match>(3, home.Object, away.Object, DateTime.Now.AddYears(2));
            return match;
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void ModifyUnexistentTest() {
            serviceToTest.ModifyMatch(fakeMatch.Object);
        }

        [TestMethod]
        public void GetAllMatchesTest() {
            Assert.AreEqual(serviceToTest.GetAllMatches().Count, 1);
        }

        


    }
}
