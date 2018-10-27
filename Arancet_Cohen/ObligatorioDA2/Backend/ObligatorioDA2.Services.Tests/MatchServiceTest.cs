using System;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.DataAccess;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Data.Repositories;
using ObligatorioDA2.Services;
using ObligatorioDA2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using Match = ObligatorioDA2.BusinessLogic.Match;
using ObligatorioDA2.Services.Exceptions;
using System.Linq;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class MatchServiceTest
    {
        private IMatchService serviceToTest;
        private IMatchRepository matchesRepo;
        private ISportRepository sportsRepo;
        private ITeamRepository teamsRepo;
        private IUserRepository usersRepo;
        private Sport sport;
        private Team teamA;
        private Team teamB;
        private Team teamC;
        private Match matchAvsB;
        private Match matchAvsC;
        private Match matchBvsC;
        private DatabaseConnection context;

        [TestInitialize]
        public void SetUp()
        {
            sport = new Sport("Soccer",true);
            teamA = new Team(1, "teamA", "photo", sport);
            teamB = new Team(2, "teamB", "photo", sport);
            teamC = new Team(3, "teamC", "photo", sport);
            matchAvsB = new Match(1, new List<Team>() { teamA, teamB }, DateTime.Now.AddDays(1), sport);
            matchAvsC = new Match(2, new List<Team>() { teamA, teamC }, DateTime.Now.AddDays(2), sport);
            matchBvsC = new Match(3, new List<Team>() { teamB, teamC }, DateTime.Now.AddDays(3), sport);
            SetUpRepository();
            context.Database.EnsureDeleted();
        }

        private void SetUpRepository()
        {

            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
               .UseInMemoryDatabase(databaseName: "MatchService")
               .Options;
            context = new DatabaseConnection(options);
            matchesRepo = new MatchRepository(context);
            sportsRepo = new SportRepository(context);
            teamsRepo = new TeamRepository(context);
            usersRepo = new UserRepository(context);
            serviceToTest = new MatchService(matchesRepo, teamsRepo, sportsRepo, usersRepo);
        }

        [TestMethod]
        public void AddMatchTest()
        {
            serviceToTest.AddMatch(matchAvsB);
            Assert.AreEqual(serviceToTest.GetAllMatches().Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchAlreadyExistsException))]
        public void AddAlreadyExistentTest()
        {
            serviceToTest.AddMatch(matchAvsB);
            Match sameMatch = new Mock<Match>(1, new List<Team>() { teamA, teamB }, matchAvsB.Date.AddDays(1), sport).Object;
            serviceToTest.AddMatch(sameMatch);
        }

        [TestMethod]
        public void AddMatchGivingIdsTest()
        {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            ICollection<int> teamsIds = matchAvsB.GetParticipants().Select(t => t.Id).ToList();
            serviceToTest.AddMatch(teamsIds, matchAvsB.Sport.Name, matchAvsB.Date);
            Assert.AreEqual(serviceToTest.GetAllMatches().Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchAlreadyExistsException))]
        public void AddGivingIdsAlreadyExistentTest()
        {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            ICollection<int> teamsIds = matchAvsB.GetParticipants().Select(t => t.Id).ToList();
            Encounter added = serviceToTest.AddMatch(teamsIds, matchAvsB.Sport.Name, matchAvsB.Date);
            Encounter sameMatch = new Match(added.Id, new List<Team>() { teamA, teamB }, matchAvsB.Date.AddDays(1), sport);
            serviceToTest.AddMatch(sameMatch);
        }

        [TestMethod]
        public void AddAssignedGivingIdsTest()
        {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            ICollection<int> teamsIds = matchAvsB.GetParticipants().Select(t => t.Id).ToList();
            Encounter stored = serviceToTest.AddMatch(3, teamsIds, matchAvsB.Sport.Name, matchAvsB.Date);
            Assert.AreEqual(stored.Date, matchAvsB.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void GetUnexistentMatchTest()
        {
            serviceToTest.GetMatch(9);
        }

        [TestMethod]
        public void GetExistentMatchTest()
        {
            serviceToTest.AddMatch(matchAvsB);
            Encounter retrieved = serviceToTest.GetMatch(matchAvsB.Id);
            Assert.AreEqual(retrieved.Id, matchAvsB.Id);

        }

        [TestMethod]
        public void GetMatchesOfSportTest()
        {
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);

            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.AddMatch(matchBvsC);
            ICollection<Encounter> matches = serviceToTest.GetAllMatches(sport.Name);
            Assert.AreEqual(matches.Count, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void GetMatchesOfNotExistingSportTest()
        {
            serviceToTest.GetAllMatches(sport.Name);
        }

        [TestMethod]
        public void GetMatchesFromTeamTest()
        {
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);

            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.AddMatch(matchAvsC);
            serviceToTest.AddMatch(matchBvsC);
            ICollection<Encounter> matches = serviceToTest.GetAllMatches(teamA.Id);
            Assert.AreEqual(2, matches.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetMatchesOfNotExistingTeamTest()
        {
            serviceToTest.GetAllMatches(matchAvsB.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void DeleteMatchTest()
        {
            serviceToTest.DeleteMatch(3);
        }

        [TestMethod]
        public void ModifyTest()
        {
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);
            serviceToTest.AddMatch(matchAvsB);
            Encounter modifiedAvsB = new Match(1, new List<Team>() { teamB, teamA }, matchAvsB.Date.AddDays(1), sport);
            SetUpRepository();
            serviceToTest.ModifyMatch(modifiedAvsB);
            Encounter modified = serviceToTest.GetMatch(matchAvsB.Id);

            Assert.AreEqual(modifiedAvsB.GetParticipants().Count, modified.GetParticipants().Count);
            Assert.AreEqual(modifiedAvsB.Date, modified.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void ModifyUnexistentTest()
        {
            serviceToTest.ModifyMatch(matchAvsC);
        }

        [TestMethod]
        public void ModifyGivingIdsTest()
        {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            serviceToTest.AddMatch(matchAvsB);
            SetUpRepository();
            Encounter modifiedAvsB = new Match(1, new List<Team>() { teamB, teamA }, matchAvsB.Date.AddDays(1), sport);
            serviceToTest.ModifyMatch(modifiedAvsB.Id, new List<int>() { teamB.Id, teamA.Id }, modifiedAvsB.Date, sport.Name);
            Encounter stored = serviceToTest.GetMatch(matchAvsB.Id);
            Assert.AreEqual(modifiedAvsB.Date, stored.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void ModifyNoMatchWithIdTest()
        {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);
            serviceToTest.ModifyMatch(5,new List<int>() { teamC.Id, teamA.Id }, matchAvsB.Date, sport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void ModifyNoTeamWithIdTest()
        {
            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.ModifyMatch(matchAvsB.Id,new List<int>() { teamC.Id, teamA.Id }, matchAvsB.Date, sport.Name);
        }

        [TestMethod]
        public void GetAllMatchesTest()
        {
            serviceToTest.AddMatch(matchAvsC);
            serviceToTest.AddMatch(matchAvsB);
            Assert.AreEqual(serviceToTest.GetAllMatches().Count, 2);
        }

        [TestMethod]
        public void ExistMatchTrueTest()
        {
            serviceToTest.AddMatch(matchAvsB);
            Assert.IsTrue(serviceToTest.Exists(matchAvsB.Id));
        }

        [TestMethod]
        public void ExistMatchFalseTest()
        {
            serviceToTest.AddMatch(matchAvsB);
            Assert.IsFalse(serviceToTest.Exists(matchAvsC.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyHasMatchException))]
        public void AwayTeamWithTwoMatchesSameDateTest()
        {
            matchAvsB.Date = matchBvsC.Date;
            serviceToTest.AddMatch(matchBvsC);
            serviceToTest.AddMatch(matchAvsB);

        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyHasMatchException))]
        public void TeamWithTwoMatchesSameDateTest()
        {
            matchAvsC.Date = matchAvsB.Date;
            serviceToTest.AddMatch(matchAvsC);
            serviceToTest.AddMatch(matchAvsB);

        }

        [TestMethod]
        public void CommentOnMatchByIdsTest()
        {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "mail@mail.com" };
            User commentarist = new User(identity, true);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            usersRepo.Add(commentarist);
            Encounter added = matchesRepo.Add(matchAvsB);
            SetUpRepository();
            serviceToTest.CommentOnMatch(added.Id, commentarist.UserName, "a Comment");
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void CommentNoMatchWithIdTest()
        {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "mail@mail.com" };
            User commentarist = new User(identity, true);
            usersRepo.Add(commentarist);
            serviceToTest.CommentOnMatch(3, commentarist.UserName, "a Comment");
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void CommentNoUserWithIdTest()
        {
            Encounter added = matchesRepo.Add(matchAvsB);
            serviceToTest.CommentOnMatch(added.Id, "usernae", "a Comment");
        }

        [TestMethod]
        public void GetMatchCommentsTest()
        {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "mail@mail.com" };
            User commentarist = new User(identity, true);
            usersRepo.Add(commentarist);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);
            Encounter added1 = matchesRepo.Add(matchAvsB);
            Encounter added2 = matchesRepo.Add(matchAvsC);
            SetUpRepository();
            serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "a Comment");
            serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "another Comment");
            serviceToTest.CommentOnMatch(added2.Id, commentarist.UserName, "a Comment");
            ICollection<Commentary> comments = serviceToTest.GetMatchCommentaries(added1.Id);
            Assert.AreEqual(comments.Count, 2);
        }

        [TestMethod]
        public void GetCommentsTest()
        {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "mail@mail.com" };
            User commentarist = new User(identity, true);
            usersRepo.Add(commentarist);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);
            Encounter added1 = matchesRepo.Add(matchAvsB);
            Encounter added2 = matchesRepo.Add(matchAvsC);
            SetUpRepository();
            serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "a Comment");
            serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "another Comment");
            serviceToTest.CommentOnMatch(added2.Id, commentarist.UserName, "a Comment");
            ICollection<Commentary> comments = serviceToTest.GetAllCommentaries();
            Assert.AreEqual(comments.Count, 3);

        }

        [TestMethod]
        public void GetCommentTest()
        {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "mail@mail.com" };
            User commentarist = new User(identity, true);
            usersRepo.Add(commentarist);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);
            Encounter added1 = matchesRepo.Add(matchAvsB);
            Encounter added2 = matchesRepo.Add(matchAvsC);
            SetUpRepository();
            Commentary comment = serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "a Comment");
            Commentary retrieved = serviceToTest.GetComment(comment.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(CommentNotFoundException))]
        public void GetNotExistingCommentTest()
        {
            Commentary retrieved = serviceToTest.GetComment(3);
        }

        [TestMethod]
        public void SetResultTest() {
            Result result = GetFakeResult();
            matchesRepo.Add(matchAvsB);
            serviceToTest.SetResult(matchAvsB.Id, result);
            Encounter retrieved = serviceToTest.GetMatch(matchAvsB.Id);
            Result retrievedResult = retrieved.Result;
            Assert.AreEqual(result.GetPositions().Count, retrievedResult.GetPositions().Count);
        }

        private Result GetFakeResult()
        {
            Result toGenerate = new Result();
            toGenerate.Add(teamA, 1);
            toGenerate.Add(teamB, 2);
            return toGenerate;
        }
    }
}
