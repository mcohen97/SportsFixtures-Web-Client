using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using Match = BusinessLogic.Match;

namespace ObligatorioDA2.DataAccess.Domain.Mappers.Tests
{
    [TestClass]
    public class MatchMapperTest
    {
        private MatchMapper testMapper;
        private MatchEntity entity;
        private Mock<BusinessLogic.Match> match;
        [TestInitialize]
        public void SetUp() {
            testMapper = new MatchMapper();
            TeamEntity homeTest = new TeamEntity { Id = 3, Name = "Nacional", Photo = "aPath" };
            TeamEntity awayTest = new TeamEntity { Id = 4, Name = "Torque", Photo = "aPath" };
            entity = new MatchEntity()
            {
                Id = 3,
                HomeTeam = homeTest,
                AwayTeam = awayTest,
                Date = DateTime.Now,
                Commentaries = new List<CommentEntity>()
            };
            Mock<Team> homeMock = new Mock<Team>(3, "Nacional", "aPath");
            Mock<Team> awayMock = new Mock<Team>(4, "Torque", "aPath");
            match = new Mock<BusinessLogic.Match>(homeMock.Object,awayMock.Object,DateTime.Now);
        }

        [TestMethod]
        public void MatchToEntityHomeTest() {
            MatchEntity converted =testMapper.ToEntity(match.Object);
            Assert.AreEqual(converted.HomeTeam.Id, entity.HomeTeam.Id);
        }
        [TestMethod]
        public void MatchToEntityAwayTest() {
            MatchEntity converted = testMapper.ToEntity(match.Object);
            Assert.AreEqual(converted.AwayTeam.Id, entity.AwayTeam.Id);

        }
        [TestMethod]
        public void MatchToEntityDateTest()
        {
            MatchEntity converted = testMapper.ToEntity(match.Object);
            Assert.AreEqual(converted.Date.ToString(), entity.Date.ToString());
        }

        [TestMethod]
        public void MatchToEntityCommentsCountTest()
        {
            UserId identity = new UserId { Name = "aName", Surname = "aSurname",
                UserName = "aUsername", Password = "aPassword",Email= "anEmail@aDomain.com" };
            Mock<User> user = new Mock<User>(identity,false);
            match.Object.AddCommentary(new Commentary("test comment", user.Object));
            MatchEntity converted = testMapper.ToEntity(match.Object);
            Assert.AreEqual(converted.Commentaries.Count, 1);   
        }

        [TestMethod]
        public void EntityToMatchHomeTest() {
            Match conversion = testMapper.ToMatch(entity);
            Assert.AreEqual(match.Object.HomeTeam.Id, conversion.HomeTeam.Id);
        }

        [TestMethod]
        public void EntityToMatchAwayTest()
        {
            Match conversion = testMapper.ToMatch(entity);
            Assert.AreEqual(match.Object.AwayTeam.Id, conversion.AwayTeam.Id);
        }

        [TestMethod]
        public void EntityToMatchDateTest()
        {
            Match conversion = testMapper.ToMatch(entity);
            Assert.AreEqual(match.Object.Date.ToString(),conversion.Date.ToString());
        }

        [TestMethod]
        public void EntityToMatchCommentsTest()
        {
            entity.Commentaries.Add(new CommentEntity()
            {
                Id = 3,
                Text = "text",
                Maker = new UserEntity
                {
                    Name = "aName",
                    Surname = "aSurname",
                    UserName = "aUsername",
                    Password = "aPassword",
                    Email = "aEmail@aDomain.com"
                }
            });
            Match conversion = testMapper.ToMatch(entity);
            Assert.AreEqual(conversion.GetAllCommentaries().Count, 1);       
        }
    }
}
