using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.BusinessLogic;
using System.Collections.Generic;
using Moq;
using ObligatorioDA2.BusinessLogic.Exceptions;
using Match = ObligatorioDA2.BusinessLogic.Match;
using System.Linq;

namespace ObligatorioDA2.BusinessLogic.Test
{
    [TestClass]
    public class MatchTest
    {
        private Team teamA;
        private Team teamB;
        private Team teamC;
        private Sport sport;
        private DateTime date;
        private Match match;
        private Mock<Commentary> commentary1;
        private Mock<Commentary> commentary2;
        private Mock<Commentary> commentary3;



        [TestInitialize]
        public void TestInitialize()
        {
            //Create mocks
            sport = new Sport("Soccer",true);
            teamA = new Team(1, "TeamA", "Photo/A", sport);
            teamB = new Team(2, "TeamB", "Photo/B", sport);
            teamC = new Team(2, "TeamC", "Photo/C", sport);
            date = new DateTime(2019, 1, 25, 13, 30, 0);
            Mock<User> commentarist = CreateUser();
            commentary1 = new Mock<Commentary>(1, "Commentary 1", commentarist.Object);
            commentary2 = new Mock<Commentary>(2, "Commentary 2", commentarist.Object);
            commentary3 = new Mock<Commentary>(3, "Commentary 3", commentarist.Object);

            //Configure mocks
            //teamA.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 1);
            //teamB.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 2);
            commentary1.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Commentary)?.Id == 1);
            commentary2.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Commentary)?.Id == 2);
            commentary3.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Commentary)?.Id == 3);

            match = new Match(3,new List<Team>() { teamA, teamB }, date, sport);
        }

        private Mock<User> CreateUser()
        {
            UserId identity = new UserId()
            {
                Name = "aName",
                Surname = "aSurname",
                UserName = "aUsername",
                Password = "aPassword",
                Email = "anEmail@aDomain.com"
            };
            Mock<User> toReturn = new Mock<User>(identity, false);
            return toReturn;
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void GetIdTest()
        {
            Assert.AreEqual(match.Id, 3);
        }

        [TestMethod]
        public void GetTeamTest()
        {
            Assert.IsTrue(match.GetParticipants().Any(t => t.Id == teamA.Id));
        }

        [TestMethod]
        public void GetDateTimeTest()
        {
            Assert.AreEqual(date, match.Date);
        }

        [TestMethod]
        public void GetSportTest()
        {
            Sport played = match.Sport;
            Assert.AreEqual("Soccer", played.Name);
        }

        [TestMethod]
        public void SetDateTimeTest()
        {
            DateTime newDate = new DateTime(2019, 2, 2, 12, 0, 0);
            match.Date = newDate;
            Assert.AreEqual(newDate, match.Date);
        }

        [TestMethod]
        public void MatchHasNotCommentaryTest()
        {
            Assert.IsFalse(match.HasCommentary(commentary1.Object));
        }

        [TestMethod]
        public void AddCommentaryTest()
        {
            match.AddCommentary(commentary1.Object);
            Assert.IsTrue(match.HasCommentary(commentary1.Object));
        }

        [TestMethod]
        public void RemoveCommentaryTest()
        {
            match.AddCommentary(commentary1.Object);
            match.RemoveCommentary(commentary1.Object);
            Assert.IsFalse(match.HasCommentary(commentary1.Object));
        }

        [TestMethod]
        public void GetAllCommentsTest()
        {
            match.AddCommentary(commentary1.Object);
            match.AddCommentary(commentary2.Object);
            match.AddCommentary(commentary3.Object);

            ICollection<Commentary> commentaries = match.GetAllCommentaries();

            Assert.AreEqual(3, commentaries.Count);
        }

        [TestMethod]
        public void RemoveAllCommentsTest()
        {
            match.AddCommentary(commentary1.Object);
            match.AddCommentary(commentary2.Object);
            match.AddCommentary(commentary3.Object);

            match.RemoveAllComments();

            Assert.IsFalse(match.HasCommentary(commentary1.Object));
            Assert.IsFalse(match.HasCommentary(commentary2.Object));
            Assert.IsFalse(match.HasCommentary(commentary3.Object));
        }

        [TestMethod]
        public void GetCommentaryTest()
        {
            match.AddCommentary(commentary1.Object);
            Commentary commentary = match.GetCommentary(commentary1.Object.Id);
            Assert.AreEqual(commentary.Text, commentary1.Object.Text);
        }

        //Exceptions

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void SetNullTeamsTest()
        {
            match = new Match(3, null, date, sport);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void RepeatedTeamsTest()
        {
            match = new Match(3, new List<Team>() { teamA, teamA }, date, sport);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void DifferentSportTeamsTest()
        {
            sport = new Sport("Basketball", true);
            teamB = new Team(2, "TeamB", "Photo/B", sport);
            match = new Match(3, new List<Team>() { teamA, teamB }, date, sport);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void LessThanTwoTeamsTest() {
            match = new Match(3, new List<Team>() {}, date, sport);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void TooManyTeamsAllowedBySport() {
            sport = new Sport("Basketball", true);
            match = new Match(3, new List<Team>() { teamA, teamB,  teamC}, date, sport);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void SetNullSportTest()
        {
            match = new Match(3,new List<Team>() { teamA, teamB }, date, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataException))]
        public void AddAlreadyExistentCommentaryTest()
        {
            match.AddCommentary(commentary1.Object);
            match.AddCommentary(commentary1.Object);
        }
    }
}