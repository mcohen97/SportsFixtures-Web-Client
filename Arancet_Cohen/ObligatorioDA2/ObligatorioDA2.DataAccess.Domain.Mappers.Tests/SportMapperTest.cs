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
            sport = new Mock<Sport>(3,"Soccer");
            Mock<Team> aTeam = new Mock<Team>("Nacional", "path");
            entity = new SportEntity()
            {
                Id = 3,
                Name = "Soccer",
                Teams = new List<TeamEntity>()
            };
            sport.Object.AddTeam(aTeam.Object);
        }

        [TestMethod]
        public void SportToEntityIdTest()
        {
            SportEntity converted = testMapper.ToEntity(sport.Object);
            Assert.AreEqual(converted.Id, entity.Id);
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
            IEnumerator<TeamEntity> teams = converted.Teams.GetEnumerator();
            teams.MoveNext();
            TeamEntity toCompare = teams.Current;
            Assert.AreEqual(toCompare.Name, "Nacional");
        }

    }
}
