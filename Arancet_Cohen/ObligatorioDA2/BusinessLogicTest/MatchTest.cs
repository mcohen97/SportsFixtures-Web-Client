using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using System.Collections.Generic;
using Moq;
using BusinessLogic.Exceptions;

namespace BusinessLogicTest
{
    [TestClass]
    public class MatchTest
    {
        private Mock<Team> teamA;
        private Mock<Team> teamB;
        private Mock<Sport> sport;
        private DateTime date;
        private BusinessLogic.Match match;
        private Mock<Commentary> commentary1;
        private Mock<Commentary> commentary2;
        private Mock<Commentary> commentary3;


        [TestInitialize]
        public void TestInitialize(){
            //Create mocks
            teamA = new Mock<Team>(1, "TeamA", "Photo/A");
            teamB = new Mock<Team>(2, "TeamB", "Photo/B");
            date =  new DateTime(2019,1,25,13,30,0);
            Mock<User> commentarist = CreateUser();
            commentary1 = new Mock<Commentary>(1, "Commentary 1",commentarist.Object);
            commentary2 = new Mock<Commentary>(2, "Commentary 2", commentarist.Object);
            commentary3 = new Mock<Commentary>(3, "Commentary 3", commentarist.Object);
            
            //Configure mocks
            teamA.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 1);
            teamB.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 2);
            commentary1.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Commentary)?.Id == 1);
            commentary2.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Commentary)?.Id == 2);
            commentary3.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Commentary)?.Id == 3);

            match = new BusinessLogic.Match(3,teamA.Object, teamB.Object, date);
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
        public void ConstructorTest(){
            Assert.IsNotNull(match);
        }

        [TestMethod]
        public void GetIdTest() {
            Assert.AreEqual(match.Id, 3);
        }

        [TestMethod]
        public void GetHomeTeamTest(){
            Assert.AreEqual(teamA.Object.Id, match.HomeTeam.Id);
        }

        [TestMethod]
        public void GetAwayTeamTest(){
            Assert.AreEqual(teamB.Object.Id, match.AwayTeam.Id);
        }

        [TestMethod]
        public void SetHomeTeamTest(){
            Mock<Team> homeTeam = new Mock<Team>(3, "HomeTeam", "photo");
            match.HomeTeam = homeTeam.Object;
            Assert.AreEqual(homeTeam.Object.Id, match.HomeTeam.Id);
        }

        [TestMethod]
        public void SetAwayTeamTest(){
            Mock<Team> awayTeam = new Mock<Team>(3, "AwayTeam", "photo");
            match.AwayTeam = awayTeam.Object;
            Assert.AreEqual(awayTeam.Object.Id, match.AwayTeam.Id);
        }

        [TestMethod]
        public void GetDateTimeTest(){
            Assert.AreEqual(date, match.Date);
        }

        [TestMethod]
        public void SetDateTimeTest(){
            DateTime newDate = new DateTime(2019,2,2,12,0,0);
            match.Date = newDate;
            Assert.AreEqual(newDate, match.Date);
        }

        [TestMethod]
        public void MatchHasNotCommentaryTest(){
            Assert.IsFalse(match.HasCommentary(commentary1.Object));
        }
       
        [TestMethod]
        public void AddCommentaryTest(){
            match.AddCommentary(commentary1.Object);
            Assert.IsTrue(match.HasCommentary(commentary1.Object));
        }

        [TestMethod]
        public void RemoveCommentaryTest(){
            match.AddCommentary(commentary1.Object);
            match.RemoveCommentary(commentary1.Object);
            Assert.IsFalse(match.HasCommentary(commentary1.Object));
        }

        [TestMethod]
        public void GetAllCommentsTest(){
            match.AddCommentary(commentary1.Object);
            match.AddCommentary(commentary2.Object);
            match.AddCommentary(commentary3.Object);

            ICollection<Commentary> commentaries = match.GetAllCommentaries();

            Assert.AreEqual(3, commentaries.Count);
        }

        [TestMethod]
        public void RemoveAllCommentsTest(){
            match.AddCommentary(commentary1.Object);
            match.AddCommentary(commentary2.Object);
            match.AddCommentary(commentary3.Object);

            match.RemoveAllComments();

            Assert.IsFalse(match.HasCommentary(commentary1.Object));
            Assert.IsFalse(match.HasCommentary(commentary2.Object));
            Assert.IsFalse(match.HasCommentary(commentary3.Object));
        }

        [TestMethod]
        public void GetCommentaryTest(){
            match.AddCommentary(commentary1.Object);
            Commentary commentary = match.GetCommentary(commentary1.Object.Id);
            Assert.AreEqual(commentary.Text, commentary1.Object.Text);
        }

        //Exceptions

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataExcpetion))]
        public void SetNullHomeTeamTest()
        {
            match.HomeTeam = null;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataExcpetion))]
        public void SetNullAwayTeamTest()
        {
            match.AwayTeam = null;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataExcpetion))]
        public void HomeEqualsAwayTest()
        {
            match.AwayTeam = teamA.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataExcpetion))]
        public void AwayEqualsHomeTest()
        {
            match.HomeTeam = teamB.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMatchDataExcpetion))]
        public void AddAlreadyExistentCommentaryTest()
        {
            match.AddCommentary(commentary1.Object);
            match.AddCommentary(commentary1.Object);
        }
    }
}