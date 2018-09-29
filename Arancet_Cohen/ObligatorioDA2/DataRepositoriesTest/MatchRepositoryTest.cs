using System;
using System.Collections.Generic;
using BusinessLogic;
using DataAccess;
using DataRepositories;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.DataAccess.Entities;
using Match = BusinessLogic.Match;

namespace DataRepositoriesTest
{
    [TestClass]
    public class MatchRepositoryTest
    {
        IMatchRepository matchesStorage;
        Mock<BusinessLogic.Match> match;
        Mock<Sport> sport;

        [TestInitialize]
        public void SetUp() {
            sport = new Mock<Sport>("Soccer");
            SetUpRepository();
            match = BuildFakeMatch(); 
            matchesStorage.Clear();
            
        }

        private void SetUpRepository() {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "MatchRepository")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            matchesStorage = new MatchRepository(context);
        }

        private Mock<BusinessLogic.Match> BuildFakeMatch()
        {
            Mock<Team> home = new Mock<Team>(3,"Manchester United","aPath");
            Mock<Team> away = new Mock<Team>(5,"Real Madrid", "aPath");
            Mock<BusinessLogic.Match> match = new Mock<BusinessLogic.Match>(3,home.Object, away.Object, DateTime.Now,sport.Object);
            return match;
        }

        [TestMethod]
        public void EmptyTest() {
            Assert.IsTrue(matchesStorage.IsEmpty());
        }

        [TestMethod]
        public void AddMatchNotemptyTest() {
            matchesStorage.Add("Soccer",match.Object);
            Assert.IsFalse(matchesStorage.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(MatchAlreadyExistsException))]
        public void AddRepeatedMatchTest() {
            matchesStorage.Add("Soccer",match.Object);
            matchesStorage.Add("Soccer",match.Object);
        }


        [TestMethod]
        public void GetMatchHomeTeamTest() {
            Match added =matchesStorage.Add("Sport",match.Object);
            Match retrieved = matchesStorage.Get(added.Id);
            Assert.AreEqual(retrieved.HomeTeam, match.Object.HomeTeam);
        }

        [TestMethod]
        public void GetMatchAwayTeamTest() {
            Match added =matchesStorage.Add("Soccer",match.Object);
            Match retrieved = matchesStorage.Get(added.Id);
            Assert.AreEqual(retrieved.AwayTeam, match.Object.AwayTeam);
        }

        [TestMethod]
        public void GetMatchCommentsTest() {
            Mock<Commentary> dummy = BuildFakeCommentary();
            match.Object.AddCommentary(dummy.Object);
            Match added =matchesStorage.Add("Sport",match.Object);
            Match retrieved = matchesStorage.Get(added.Id);
            Assert.AreEqual(retrieved.GetAllCommentaries().Count, 1);
        }

        private Mock<Commentary> BuildFakeCommentary()
        {
            UserId identity = new UserId() {
                Name = "aName",
                Surname = "aSurname",
                UserName = "aUsername",
                Password = "aPassword",
                Email = "anEmail@aDomain.com"
            };
            Mock<User> somebody = new Mock<User>(identity,false);
            Mock<Commentary> comment = new Mock<Commentary>("Some comment", somebody.Object);
            return comment;
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void GetMatchNotFoundTest()
        {
            Match added =matchesStorage.Add("Sport",match.Object);
            Match retrieved = matchesStorage.Get(4);
        }

        [TestMethod]
        public void GetAllTest() {
            matchesStorage.Add("Soccer",match.Object);
            ICollection<Match> all = matchesStorage.GetAll();
            Assert.AreEqual(all.Count, 1);
        }

        [TestMethod]
        public void ModifyTest() {
            matchesStorage.Add("Soccer",match.Object);

            Mock<Match> modified = BuildModifiedFakeMatch();
            SetUpRepository();
            matchesStorage.Modify("Soccer",modified.Object);
            Match retrieved = matchesStorage.Get(3);
            Assert.AreEqual(retrieved.AwayTeam, modified.Object.AwayTeam);
            Assert.AreEqual(retrieved.HomeTeam, modified.Object.HomeTeam);
            Assert.AreEqual(retrieved.Date, modified.Object.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void ModifyUnexistentItemTest() {
            Mock<Team> home = new Mock<Team>("Manchester United", "aPath");
            Mock<Team> away = new Mock<Team>("Bayern Munich", "aPath");
            Mock<BusinessLogic.Match> match = new Mock<BusinessLogic.Match>(7, home.Object, away.Object, DateTime.Now.AddYears(2),sport.Object);
            matchesStorage.Modify("Soccer",match.Object);
        }

        private Mock<BusinessLogic.Match> BuildModifiedFakeMatch()
        {
            Mock<Team> home = new Mock<Team>("Manchester United", "aPath");
            Mock<Team> away = new Mock<Team>("Bayern Munich", "aPath");
            Mock<BusinessLogic.Match> match = new Mock<BusinessLogic.Match>(3, home.Object, away.Object, DateTime.Now.AddYears(2),sport.Object);
            return match;
        }

        [TestMethod]
        public void ExistsTest() {
            Match added = matchesStorage.Add("Soccer",match.Object);
            bool exists = matchesStorage.Exists(added.Id);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void DoesNotExistTest() {
            bool exists = matchesStorage.Exists(5);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void DeleteTest() {
            Match added =matchesStorage.Add("Sport",match.Object);
            matchesStorage.Delete(match.Object.Id);
            bool exists = matchesStorage.Exists(added.Id);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void DeleteUnexistentTest() {
            matchesStorage.Delete(match.Object.Id);
        }

    }
}
