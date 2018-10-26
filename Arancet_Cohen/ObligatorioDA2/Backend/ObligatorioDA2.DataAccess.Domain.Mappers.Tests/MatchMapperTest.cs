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
        private ICollection<MatchTeam> playingTeams;
        private Mock<BusinessLogic.Match> match;
        [TestInitialize]
        public void SetUp()
        {
            testMapper = new MatchMapper();
            SportEntity testSport = new SportEntity() { Name = "Soccer", IsTwoTeams = true };
            TeamEntity homeTest = new TeamEntity { TeamNumber = 3, SportEntityName = "Soccer", Sport =testSport,Name = "Nacional", Photo = "aPath" };
            TeamEntity awayTest = new TeamEntity { TeamNumber = 4, SportEntityName = "Soccer", Sport = testSport,Name = "Torque", Photo = "aPath" };
            entity = new MatchEntity()
            {
                Id = 3,
                Date = DateTime.Now,
                SportEntity = testSport,
                Commentaries = new List<CommentEntity>()
            };
            MatchTeam homeRel = new MatchTeam() { Team = homeTest, TeamNumber = 3, Match = entity, MatchId = 3 };
            MatchTeam awayRel = new MatchTeam() { Team = awayTest, TeamNumber = 4, Match = entity, MatchId = 3 };
            playingTeams = new List<MatchTeam>() { homeRel, awayRel };
            Team homeMock = new Team(3, "Nacional", "aPath", new Sport("Soccer",true));
            Team awayMock = new Team(4, "Torque", "aPath", new Sport("Soccer",true));
            Sport sport = new Sport("Soccer",true);
            match = new Mock<Match>(new List<Team>() { homeMock, awayMock }, DateTime.Now, sport);
        }

        [TestMethod]
        public void GetMatchTeamsTest()
        {
            ICollection<MatchTeam> teams = testMapper.ConvertParticipants(match.Object);
            Assert.AreEqual(teams.Count, match.Object.GetParticipants().Count);
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
            UserId identity = new UserId
            {
                Name = "aName",
                Surname = "aSurname",
                UserName = "aUsername",
                Password = "aPassword",
                Email = "anEmail@aDomain.com"
            };
            Mock<User> user = new Mock<User>(identity, false);
            match.Object.AddCommentary(new Commentary("test comment", user.Object));
            MatchEntity converted = testMapper.ToEntity(match.Object);
            Assert.AreEqual(converted.Commentaries.Count, 1);
        }

        [TestMethod]
        public void EntityToMatchTeamsTest()
        {
            Match conversion = testMapper.ToMatch(entity,playingTeams);
            Assert.AreEqual(match.Object.GetParticipants().Count, playingTeams.Count);
        }

        [TestMethod]
        public void EntityToMatchDateTest()
        {
            Match conversion = testMapper.ToMatch(entity,playingTeams);
            Assert.AreEqual(match.Object.Date.ToString(), conversion.Date.ToString());
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
            Match conversion = testMapper.ToMatch(entity,playingTeams);
            Assert.AreEqual(conversion.GetAllCommentaries().Count, 1);
        }
    }
}
