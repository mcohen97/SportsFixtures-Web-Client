﻿using System;
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
using ObligatorioDA2.Services.Exceptions;

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

        [TestInitialize]
        public void SetUp() {
            sport = new Sport("Soccer");
            teamA = new Team(1, "teamA", "photo", sport);
            teamB = new Team(2, "teamB", "photo", sport);
            teamC = new Team(3, "teamC", "photo", sport);
            matchAvsB = new Match(1, teamA, teamB, DateTime.Now.AddDays(1), sport);
            matchAvsC = new Match(2, teamA, teamC, DateTime.Now.AddDays(2), sport);
            matchBvsC = new Match(3, teamB, teamC, DateTime.Now.AddDays(3), sport);
            SetUpRepository();
            matchesRepo.Clear();
            sportsRepo.Clear();
            teamsRepo.Clear();
            usersRepo.Clear();
        }

        private void SetUpRepository() {

            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
               .UseInMemoryDatabase(databaseName: "MatchService")
               .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            matchesRepo = new MatchRepository(context);
            sportsRepo = new SportRepository(context);
            teamsRepo = new TeamRepository(context);
            usersRepo = new UserRepository(context);
            serviceToTest = new MatchService(matchesRepo, teamsRepo, sportsRepo, usersRepo);
            context.Comments.RemoveRange(context.Comments);
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
            Match sameMatch = new Mock<Match>(1, teamA, teamB, matchAvsB.Date.AddDays(1), sport).Object;
            serviceToTest.AddMatch(sameMatch);
        }

        [TestMethod]
        public void AddMatchGivingIdsTest() {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            serviceToTest.AddMatch(matchAvsB.HomeTeam.Id, matchAvsB.AwayTeam.Id, matchAvsB.Sport.Name, matchAvsB.Date);
            Assert.AreEqual(serviceToTest.GetAllMatches().Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchAlreadyExistsException))]
        public void AddGivingIdsAlreadyExistentTest() {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            Match added = serviceToTest.AddMatch(matchAvsB.HomeTeam.Id, matchAvsB.AwayTeam.Id, matchAvsB.Sport.Name, matchAvsB.Date);
            Match sameMatch = new Mock<Match>(added.Id, teamA, teamB, matchAvsB.Date.AddDays(1), sport).Object;
            serviceToTest.AddMatch(sameMatch);
        }

        [TestMethod]
        public void AddAssignedGivingIdsTest() {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            Match stored = serviceToTest.AddMatch(3, matchAvsB.HomeTeam.Id, matchAvsB.AwayTeam.Id, matchAvsB.Sport.Name, matchAvsB.Date);
            Assert.AreEqual(stored.Date, matchAvsB.Date);
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
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);

            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.AddMatch(matchBvsC);
            ICollection<Match> matches = serviceToTest.GetAllMatches(sport.Name);
            Assert.AreEqual(matches.Count, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void GetMatchesOfNotExistingSportTest() {
            serviceToTest.GetAllMatches(sport.Name);
        }

        [TestMethod]
        public void GetMatchesFromTeamTest() {
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);

            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.AddMatch(matchAvsC);
            serviceToTest.AddMatch(matchBvsC);
            ICollection<Match> matches = serviceToTest.GetAllMatches(teamA.Id);
            Assert.AreEqual(2, matches.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetMatchesOfNotExistingTeamTest() {
            serviceToTest.GetAllMatches(matchAvsB.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void DeleteMatchTest() {
            serviceToTest.DeleteMatch(3);
        }

        [TestMethod]
        public void ModifyTest() {
            serviceToTest.AddMatch(matchAvsB);
            Match modifiedAvsB = new Match(1, teamB, teamA, matchAvsB.Date.AddDays(1), sport);
            SetUpRepository();
            serviceToTest.ModifyMatch(modifiedAvsB);
            Match modified = serviceToTest.GetMatch(matchAvsB.Id);

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
        public void ModifyGivingIdsTest() {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            serviceToTest.AddMatch(matchAvsB);
            SetUpRepository();
            Match modifiedAvsB = new Match(1, teamB, teamA, matchAvsB.Date.AddDays(1), sport);
            serviceToTest.ModifyMatch(modifiedAvsB.Id, teamB.Id, teamA.Id, modifiedAvsB.Date, sport.Name);
            Match stored = serviceToTest.GetMatch(matchAvsB.Id);
            Assert.AreEqual(modifiedAvsB.Date, stored.Date);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void ModifyNoMatchWithIdTest() {
            sportsRepo.Add(sport);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);
            serviceToTest.ModifyMatch(5, teamC.Id, teamA.Id, matchAvsB.Date, sport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void ModifyNoTeamWithIdTest() {
            serviceToTest.AddMatch(matchAvsB);
            serviceToTest.ModifyMatch(matchAvsB.Id, teamC.Id, teamA.Id, matchAvsB.Date, sport.Name);
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
        public void HomeTeamWithTwoMatchesSameDateTest()
        {
            matchAvsC.Date = matchAvsB.Date;
            serviceToTest.AddMatch(matchAvsC);
            serviceToTest.AddMatch(matchAvsB);

        }

        [TestMethod]
        public void CommentOnMatchByIdsTest() {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "mail@mail.com" };
            User commentarist = new User(identity, true);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            usersRepo.Add(commentarist);
            Match added = matchesRepo.Add(matchAvsB);
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
        public void CommentNoUserWithIdTest() {
            Match added = matchesRepo.Add(matchAvsB);
            serviceToTest.CommentOnMatch(added.Id, "usernae", "a Comment");
        }

        [TestMethod]
        public void GetMatchCommentsTest() {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "mail@mail.com" };
            User commentarist = new User(identity, true);
            usersRepo.Add(commentarist);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);
            Match added1 = matchesRepo.Add(matchAvsB);
            Match added2 = matchesRepo.Add(matchAvsC);
            SetUpRepository();
            serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "a Comment");
            serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "another Comment");
            serviceToTest.CommentOnMatch(added2.Id, commentarist.UserName, "a Comment");
            ICollection<Commentary> comments = serviceToTest.GetMatchCommentaries(added1.Id);
            Assert.AreEqual(comments.Count, 2);
        }

        [TestMethod]
        public void GetCommentsTest() {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "mail@mail.com" };
            User commentarist = new User(identity, true);
            usersRepo.Add(commentarist);
            teamsRepo.Add(teamA);
            teamsRepo.Add(teamB);
            teamsRepo.Add(teamC);
            Match added1 = matchesRepo.Add(matchAvsB);
            Match added2 = matchesRepo.Add(matchAvsC);
            SetUpRepository();
            serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "a Comment");
            serviceToTest.CommentOnMatch(added1.Id, commentarist.UserName, "another Comment");
            serviceToTest.CommentOnMatch(added2.Id, commentarist.UserName, "a Comment");
            ICollection<Commentary> comments = serviceToTest.GetAllCommentaries();
            Assert.AreEqual(comments.Count, 3);

        }

    }
}
