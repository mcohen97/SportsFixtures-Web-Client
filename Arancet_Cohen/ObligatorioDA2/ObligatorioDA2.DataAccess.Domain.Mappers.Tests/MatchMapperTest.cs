using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;
using System;
using System.Collections.Generic;

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
    }
}
