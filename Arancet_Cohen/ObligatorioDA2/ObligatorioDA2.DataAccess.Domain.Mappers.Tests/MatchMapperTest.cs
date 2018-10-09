using ObligatorioDA2.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.Data.Entities;
using System;
using System.Collections.Generic;
using Match = ObligatorioDA2.BusinessLogic.Match;

namespace ObligatorioDA2.Data.DomainMappers.Mappers.Tests
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
            TeamEntity homeTest = new TeamEntity { Identity =3,SportEntityName = "Soccer", Name = "Nacional", Photo = "aPath" };
            TeamEntity awayTest = new TeamEntity { Identity =4,SportEntityName= "Soccer", Name = "Torque", Photo = "aPath" };
            entity = new MatchEntity()
            {
                Id = 3,
                HomeTeam = homeTest,
                AwayTeam = awayTest,
                Date = DateTime.Now,
                SportEntity = new SportEntity() {Name="Soccer"},
                Commentaries = new List<CommentEntity>()
            };
            Team homeMock = new Team(3, "Nacional", "aPath",new Sport("Soccer"));
            Team awayMock = new Team(4, "Torque", "aPath",new Sport("Soccer"));
            Sport sport = new Sport( "Soccer");
            match = new Mock<BusinessLogic.Match>(homeMock,awayMock,DateTime.Now,sport);
        }

        [TestMethod]
        public void MatchToEntityHomeTest() {
            MatchEntity converted =testMapper.ToEntity(match.Object);
            Assert.AreEqual(converted.HomeTeam.Name, entity.HomeTeam.Name);
        }
        [TestMethod]
        public void MatchToEntityAwayTest() {
            MatchEntity converted = testMapper.ToEntity(match.Object);
            Assert.AreEqual(converted.AwayTeam.Name, entity.AwayTeam.Name);
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
