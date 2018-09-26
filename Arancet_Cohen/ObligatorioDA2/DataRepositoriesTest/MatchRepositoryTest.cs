using System;
using System.Collections.Generic;
using BusinessLogic;
using DataAccess;
using DataRepositories;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;
using Match = BusinessLogic.Match;

namespace DataRepositoriesTest
{
    [TestClass]
    public class MatchRepositoryTest
    {
        IMatchRepository matchesStorage;
        Mock<BusinessLogic.Match> match;

        [TestInitialize]
        public void SetUp() {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "TeamRepository")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            GenericRepository<MatchEntity> genericRepo = new GenericRepository<MatchEntity>(context);
            matchesStorage = new MatchRepository(context);
            match = BuildFakeMatch(); 
            matchesStorage.Clear();
        }

        private Mock<BusinessLogic.Match> BuildFakeMatch()
        {
            Mock<Team> home = new Mock<Team>("Manchester United","aPath");
            Mock<Team> away = new Mock<Team>("Real Madrid", "aPath");
            Mock<BusinessLogic.Match> match = new Mock<BusinessLogic.Match>(3,home.Object, away.Object, DateTime.Now);
            return match;
        }

        [TestMethod]
        public void EmptyTest() {
            Assert.IsTrue(matchesStorage.IsEmpty());
        }

        [TestMethod]
        public void AddMatchNotemptyTest() {
            matchesStorage.Add(match.Object);
            Assert.IsFalse(matchesStorage.IsEmpty());
        }

        [TestMethod]
        public void GetMatchHomeTeamTest() {
            matchesStorage.Add(match.Object);
            Match retrieved = matchesStorage.Get(3);
            Assert.AreEqual(retrieved.HomeTeam, match.Object.HomeTeam);
        }

        [TestMethod]
        public void GetMatchAwayTeamTest() {
            matchesStorage.Add(match.Object);
            Match retrieved = matchesStorage.Get(3);
            Assert.AreEqual(retrieved.AwayTeam, match.Object.AwayTeam);
        }

        [TestMethod]
        public void GetMatchCommentsTest() {
            Mock<Commentary> dummy = BuildFakeCommentary();
            match.Object.AddCommentary(dummy.Object);
            matchesStorage.Add(match.Object);
            Match retrieved = matchesStorage.Get(3);
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
        private void GetAllTest() {
            matchesStorage.Add(match.Object);
            ICollection<Match> all = matchesStorage.GetAll();
            Assert.AreEqual(all.Count, 1);
        }

        [TestMethod]
        private void ModifyTest() {
            matchesStorage.Add(match.Object);
            Mock<Match> modified = BuildModifiedFakeMatch();
            matchesStorage.Add(modified.Object);
            Match retrieved = matchesStorage.Get(3);
            Assert.AreEqual(retrieved.AwayTeam, modified.Object.AwayTeam);
            Assert.AreEqual(retrieved.HomeTeam, modified.Object.HomeTeam);
            Assert.AreEqual(retrieved.Date, modified.Object.Date);
        }

        private Mock<BusinessLogic.Match> BuildModifiedFakeMatch()
        {
            Mock<Team> home = new Mock<Team>("Manchester United", "aPath");
            Mock<Team> away = new Mock<Team>("Bayern Munich", "aPath");
            Mock<BusinessLogic.Match> match = new Mock<BusinessLogic.Match>(3, home.Object, away.Object, DateTime.Now.AddYears(2));
            return match;
        }
    }
}
