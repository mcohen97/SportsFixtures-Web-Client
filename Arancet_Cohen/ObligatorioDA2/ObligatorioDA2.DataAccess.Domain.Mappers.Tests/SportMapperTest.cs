using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;
using System.Collections.Generic;

namespace ObligatorioDA2.DataAccess.Domain.Mappers.Tests
{
    [TestClass]
    public class SportMapperTest
    {
        private SportMapper testMapper;
        private Mock<Sport> sport;
        private SportEntity entity;
        [TestInitialize]
        public void SetUp() {
            testMapper = new SportMapper();
            sport = new Mock<Sport>("Soccer");
            Mock<Team> aTeam = new Mock<Team>("Nacional", "path");
            sport.Setup(s => s.GetTeams()).Returns(new List<Team>() {aTeam.Object });
        }

        [TestMethod]
        public void SportToEntityNameTest() {
            SportEntity converted = testMapper.ToEntity(sport.Object);
            Assert.AreEqual(converted.Name, entity.Name);
         }

        [TestMethod]
        public void SportToEntityTeamsCountTest() {
            SportEntity converted = testMapper.ToEntity(sport.Object);
            Assert.AreEqual(converted.Teams.Count, 1);
        }

        [TestMethod]
        public void SportToEntityTeamsTest() {
            SportEntity converted = testMapper.ToEntity(sport.Object);
            TeamEntity toCompare = converted.Teams.GetEnumerator().Current;
            Assert.AreEqual(toCompare.Name, "Nacional");
        }

    }
}
