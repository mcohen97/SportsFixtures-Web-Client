using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;

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
        private TeamDto testDto;

        [TestInitialize]
        public void SetUp() {
            sports = new Mock<ISportRepository>();
            teams = new Mock<ITeamRepository>();
            auth = new Mock<IAuthenticationService>();
            testService = new TeamService(sports.Object, teams.Object, auth.Object);
            testSport = new Sport("football", true);
            testTeam = new Team(1,"Real Madrid", "photo", testSport);
            testDto = new TeamDto() { name = testTeam.Name, photo = testTeam.PhotoPath, sportName = testSport.Name };
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

        [TestMethod]
        public void AddTeamTest() {
            auth.Setup(r => r.IsLoggedIn()).Returns(true);
            auth.Setup(r => r.HasAdminPermissions()).Returns(true);
            sports.Setup(r => r.Get(testSport.Name)).Returns(testSport);
            teams.Setup(r => r.Add(testTeam)).Returns(testTeam);

            Team added =testService.AddTeam(testDto);

            auth.VerifyAll();
            sports.VerifyAll();
            teams.VerifyAll();
            Assert.AreEqual(testTeam.Id, added.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void AddTeamNotExistentSportTest() {
            sports.Setup(r => r.Get(It.IsAny<string>())).Throws(new SportNotFoundException());
            teams.Setup(r => r.Add(testTeam)).Returns(testTeam);

            Team added = testService.AddTeam(testDto);    
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void AddTeamNotLoggedException() {
            auth.Setup(r => r.IsLoggedIn()).Returns(false);
            auth.Setup(r => r.HasAdminPermissions()).Returns(false);

            Team added = testService.AddTeam(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void AddTeamNotAuthorizedException()
        {
            auth.Setup(r => r.IsLoggedIn()).Returns(true);
            auth.Setup(r => r.HasAdminPermissions()).Returns(false);

            Team added = testService.AddTeam(testDto);
        }
    }
}
