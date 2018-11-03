using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class TeamServiceTest
    {

        private ITeamService testService;
        private Mock<ISportRepository> sports;
        private Mock<ITeamRepository> teams;
        private Mock<IAuthenticationService> auth;
        private Sport testSport;
        private Team testTeam;

        [TestInitialize]
        public void SetUp() {
            sports = new Mock<ISportRepository>();
            teams = new Mock<ITeamRepository>();
            testService = new TeamService(sports.Object, teams.Object, auth.Object);
            testSport = new Sport("football", true);
            testTeam = new Team(1,"Real Madrid", "photo", testSport);
        }

        [TestMethod]
        public void GetTeamTest() {
            teams.Setup(r => r.Get(1)).Returns(testTeam);
            Team fetched = testService.GetTeam(1);
            Assert.AreEqual(testTeam.Name,fetched.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetTeamNotExistent() {
            teams.Setup(r => r.Get(It.IsAny<int>())).Throws(new TeamNotFoundException());
            testService.GetTeam(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetTeamNotLoggedTest() {
            teams.Setup(r => r.Get(1)).Returns(testTeam);
            auth.Setup(r => r.IsLoggedIn()).Returns(false);
            testService.GetTeam(1);
        }

    }
}
