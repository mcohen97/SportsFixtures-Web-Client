using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.DataAccess.Domain.Mappers;

namespace ObligatorioDA2.DataAccess.Domain.Mappers.Tests
{
    [TestClass]
    public class TeamMapperTest
    {
        private Mock<Team> mockTeam;
        private Mock<TeamEntity> mockTeamEntity;
        private TeamMapper teamMapper;

        [TestInitialize]
        public void TestInitialize() {
            mockTeam = new Mock<Team>("DreamTeam", "/MyResource/DreamTeam.png");
            mockTeamEntity = new Mock<TeamEntity>("DreamTeam", "/MyResource/DreamTeam.png");
            teamMapper = new TeamMapper();
        }

        [TestMethod]
        public void TeamToEntityTest(){
            TeamEntity convertedTeam = teamMapper.ToEntity(mockTeam.Object);
            Assert.AreEqual(convertedTeam.Name, mockTeam.Object.Name);
            Assert.AreEqual(convertedTeam.Photo, mockTeam.Object.Photo);
        }

        [TestMethod]
        public void EntityToTeamTest(){
            Team convertedTeam = teamMapper.ToTeam(mockTeamEntity.Object);
            Assert.AreEqual(convertedTeam.Name, mockTeamEntity.Object.Name);
            Assert.AreEqual(convertedTeam.Photo, mockTeamEntity.Object.Photo);
        }
    }
}