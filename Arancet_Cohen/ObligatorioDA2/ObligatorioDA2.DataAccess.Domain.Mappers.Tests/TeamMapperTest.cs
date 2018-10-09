using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Entities;
using ObligatorioDA2.Data.DomainMappers.Mappers;

namespace ObligatorioDA2.Data.DomainMappers.Mappers.Tests
{
    [TestClass]
    public class TeamMapperTest
    {
        private Mock<Team> mockTeam;
        private Mock<TeamEntity> mockTeamEntity;
        private TeamMapper teamMapper;

        [TestInitialize]
        public void TestInitialize() {
            mockTeam = new Mock<Team>(1, "DreamTeam", "/MyResource/DreamTeam.png");
            mockTeamEntity = new Mock<TeamEntity>(1, "DreamTeam", "/MyResource/DreamTeam.png");
            teamMapper = new TeamMapper();
        }


        [TestMethod]
        public void EntityToTeamTest(){
            Team convertedTeam = teamMapper.ToTeam(mockTeamEntity.Object);
            Assert.AreEqual(convertedTeam.Name, mockTeamEntity.Object.Name);
            Assert.AreEqual(convertedTeam.Photo, mockTeamEntity.Object.Photo);
        }
    }
}