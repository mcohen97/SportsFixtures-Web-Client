using System;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.DataAccess;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Entities;
using Match = ObligatorioDA2.BusinessLogic.Match;
using System.Data.Common;

namespace DataRepositoriesTest
{
    [TestClass]
    public class MatchRepositoryTest
    {
        private IMatchRepository matchesStorage;
        private ISportRepository sportsStorage;
        private Match match;
        private Sport sport;
        DatabaseConnection context;

        [TestInitialize]
        public void SetUp()
        {
            sport = new Sport("Soccer",true);
            SetUpRepository();
            match = BuildFakeMatch();
            context.Database.EnsureDeleted();
            //context.MatchTeams.RemoveRange(context.MatchTeams);
            //context.Comments.RemoveRange(context.Comments);
            //matchesStorage.Clear();
            //sportsStorage.Clear();
        }

        private void SetUpRepository()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "MatchRepositoryTest")
                .Options;
            context = new DatabaseConnection(options);
            matchesStorage = new MatchRepository(context);
            sportsStorage = new SportRepository(context);
        }

        private void CreateDisconnectedDatabase()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "MatchDisconnectedTest")
                .Options;
            Mock<DatabaseConnection> contextMock = new Mock<DatabaseConnection>(options);
            Mock<DbException> toThrow = new Mock<DbException>();
            contextMock.Setup(c => c.Matches).Throws(toThrow.Object);
            contextMock.Setup(c => c.Comments).Throws(toThrow.Object);
            matchesStorage = new MatchRepository(contextMock.Object);
        }

        private Match BuildFakeMatch()
        {
            Team home = new Team(3, "Manchester United", "aPath", sport);
            Team away = new Team(5, "Real Madrid", "aPath", sport);
            Match match = new Match(3, new List<Team>() { home, away }, DateTime.Now, sport);
            return match;
        }

        [TestMethod]
        public void EmptyTest()
        {
            Assert.IsTrue(matchesStorage.IsEmpty());
        }

        [TestMethod]
        public void ClearTest() {
            matchesStorage.Add(match);
            matchesStorage.Clear();
            Assert.IsTrue(matchesStorage.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void EmptyNoDataAccessTest()
        {
            CreateDisconnectedDatabase();
            matchesStorage.IsEmpty();
        }

        [TestMethod]
        public void AddMatchNotemptyTest()
        {
            matchesStorage.Add(match);
            Assert.IsFalse(matchesStorage.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(MatchAlreadyExistsException))]
        public void AddRepeatedMatchTest()
        {
            matchesStorage.Add(match);
            SetUpRepository();
            matchesStorage.Add(match);
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void AddMatchNoAccessTest()
        {
            CreateDisconnectedDatabase();
            matchesStorage.Add(match);
        }

        [TestMethod]
        public void GetMatchTeamsTest()
        {
            matchesStorage.Add(match);
            Match retrieved = matchesStorage.Get(match.Id);
            Assert.AreEqual(retrieved.GetParticipants().Count, match.GetParticipants().Count);
        }

        [TestMethod]
        public void GetMatchCommentsTest()
        {
            Mock<Commentary> dummy = BuildFakeCommentary();
            match.AddCommentary(dummy.Object);
            matchesStorage.Add(match);
            Match retrieved = matchesStorage.Get(match.Id);
            Assert.AreEqual(retrieved.GetAllCommentaries().Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void GetMatchNotFoundTest()
        {
            matchesStorage.Add(match);
            Match retrieved = matchesStorage.Get(4);
        }


        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void GetMatchNoAccessTest()
        {
            CreateDisconnectedDatabase();
            matchesStorage.Get(2);
        }

        [TestMethod]
        public void GetCommentsTest()
        {
            Mock<Commentary> dummy = BuildFakeCommentary();
            Mock<Team> home = new Mock<Team>(3, "Manchester United", "aPath", sport);
            Mock<Team> away = new Mock<Team>(5, "Real Madrid", "aPath", sport);
            Match match = new Match(3, new List<Team>() { home.Object, away.Object }, DateTime.Now, sport);

            matchesStorage.Add(match);
            matchesStorage.CommentOnMatch(3, dummy.Object);

            ICollection<Commentary> allComments = matchesStorage.GetComments();
            Assert.AreEqual(1, allComments.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void GetCommentsNoAccessTest()
        {
            CreateDisconnectedDatabase();
            matchesStorage.GetComments();
        }

        [TestMethod]
        public void GetCommentTest()
        {
            Mock<Commentary> dummy = BuildFakeCommentary();
            Mock<Team> home = new Mock<Team>(3, "Manchester United", "aPath", sport);
            Mock<Team> away = new Mock<Team>(5, "Real Madrid", "aPath", sport);
            Match match = new Match(3, new List<Team>() { home.Object, away.Object }, DateTime.Now, sport);

            matchesStorage.Add(match);
            Commentary added = matchesStorage.CommentOnMatch(3, dummy.Object);
            Commentary retrieved = matchesStorage.GetComment(added.Id);
            Assert.AreEqual(dummy.Object.Text, retrieved.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(CommentNotFoundException))]
        public void GetNotExistingCommentTest()
        {
            matchesStorage.GetComment(3);
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void GetCommentNoAccessTest()
        {
            CreateDisconnectedDatabase();
            matchesStorage.GetComment(3);
        }

        private Mock<Commentary> BuildFakeCommentary()
        {
            UserId identity = new UserId()
            {
                Name = "aName",
                Surname = "aSurname",
                UserName = "aUsername",
                Password = "aPassword",
                Email = "anEmail@aDomain.com"
            };
            Mock<User> somebody = new Mock<User>(identity, false);
            Mock<Commentary> comment = new Mock<Commentary>("Some comment", somebody.Object);
            return comment;
        }

        [TestMethod]
        public void GetAllTest()
        {
            matchesStorage.Add(match);
            ICollection<Match> all = matchesStorage.GetAll();
            Assert.AreEqual(all.Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void GetAllNoAccessTest()
        {
            CreateDisconnectedDatabase();
            matchesStorage.GetAll();
        }

        [TestMethod]
        public void ModifyTest()
        {
            matchesStorage.Add(match);
            Mock<Match> modified = BuildModifiedFakeMatch();
            SetUpRepository();
            matchesStorage.Modify(modified.Object);
            Match retrieved = matchesStorage.Get(3);
            Assert.AreEqual(retrieved.GetParticipants().Count, modified.Object.GetParticipants().Count);
            Assert.AreEqual(retrieved.Date, modified.Object.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void ModifyUnexistentItemTest()
        {
            Mock<Team> home = new Mock<Team>(3, "Manchester United", "aPath", sport);
            Mock<Team> away = new Mock<Team>(4, "Bayern Munich", "aPath", sport);
            Mock<Match> match = new Mock<Match>(7,new List<Team>() { home.Object, away.Object }, DateTime.Now.AddYears(2), sport);
            matchesStorage.Modify(match.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void ModifyNoAccessTest()
        {
            CreateDisconnectedDatabase();
            Mock<Team> home = new Mock<Team>(3, "Manchester United", "aPath", sport);
            Mock<Team> away = new Mock<Team>(4, "Bayern Munich", "aPath", sport);
            Mock<Match> match = new Mock<Match>(7, new List<Team>() { home.Object, away.Object }, DateTime.Now.AddYears(2), sport);
            matchesStorage.Modify(match.Object);
        }


        private Mock<Match> BuildModifiedFakeMatch()
        {
            Mock<Team> home = new Mock<Team>(3, "Manchester United", "aPath", sport);
            Mock<Team> away = new Mock<Team>(4, "Bayern Munich", "aPath", sport);
            Mock<Match> match = new Mock<Match>(3,new List<Team>() { home.Object, away.Object }, DateTime.Now.AddYears(2), sport);
            return match;
        }

        [TestMethod]
        public void ExistsTest()
        {
            matchesStorage.Add(match);
            bool exists = matchesStorage.Exists(match.Id);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void DoesNotExistTest()
        {
            bool exists = matchesStorage.Exists(5);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void ExistsNoAccessTest()
        {
            CreateDisconnectedDatabase();
            matchesStorage.Exists(5);
        }

        [TestMethod]
        public void DeleteTest()
        {
            matchesStorage.Add(match);
            matchesStorage.Delete(match.Id);
            bool exists = matchesStorage.Exists(match.Id);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void DeleteUnexistentTest()
        {
            matchesStorage.Delete(match.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void DeleteNoAccessTest()
        {
            CreateDisconnectedDatabase();
            matchesStorage.Delete(5);
        }

    }
}
