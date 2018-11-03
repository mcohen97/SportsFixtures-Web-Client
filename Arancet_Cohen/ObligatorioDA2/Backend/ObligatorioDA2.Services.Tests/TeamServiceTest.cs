using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;
using System.Collections.Generic;

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
            testDto = new TeamDto() {id=1, name = testTeam.Name, photo = testTeam.PhotoPath, sportName = testSport.Name };
        }

        [TestMethod]
        public void GetTeamTest() {
            auth.Setup(r => r.IsLoggedIn()).Returns(true);
            teams.Setup(r => r.Get(1)).Returns(testTeam);
            Team fetched = testService.GetTeam(1);
            Assert.AreEqual(testTeam.Name,fetched.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetTeamNotExistent() {
            auth.Setup(r => r.IsLoggedIn()).Returns(true);
            teams.Setup(r => r.Get(It.IsAny<int>())).Throws(new TeamNotFoundException());
            testService.GetTeam(1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]
        public void GetTeamNotLoggedTest() {
            teams.Setup(r => r.Get(1)).Returns(testTeam);
            LogOut();
            testService.GetTeam(1);
        }

        [TestMethod]
        public void AddTeamTest() {
            GrantAdminPermissions();
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
        public void AddTeamInvalidDataTest()
        {
            GrantAdminPermissions();
            sports.Setup(r => r.Get(testSport.Name)).Returns(testSport);
            teams.Setup(r => r.Add(testTeam)).Returns(testTeam);

            testDto.name = null;
            Team added = testService.AddTeam(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void AddTeamNotExistentSportTest() {
            GrantAdminPermissions();
            sports.Setup(r => r.Get(It.IsAny<string>())).Throws(new SportNotFoundException());
            teams.Setup(r => r.Add(testTeam)).Returns(testTeam);

            Team added = testService.AddTeam(testDto);    
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]
        public void AddTeamNotLoggedException() {
            LogOut();
            Team added = testService.AddTeam(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NoPermissionsException))]
        public void AddTeamNotAuthorizedException()
        {
            GrantFollowerPermissions();
            Team added = testService.AddTeam(testDto);
        }

        [TestMethod]
        public void ModifyTeamTest() {
            GrantAdminPermissions();
            teams.Setup(r => r.Get(It.IsAny<int>())).Returns(testTeam);
            testDto.name = "Manchester United";
            Team modified = testService.Modify(testDto);
            teams.Verify(r => r.Modify(It.IsAny<Team>()), Times.Once);
            Assert.AreEqual(testDto.name, modified.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void ModifyTeamNotFoundTest()
        {
            GrantAdminPermissions();
            teams.Setup(r => r.Get(It.IsAny<int>())).Throws(new TeamNotFoundException());
            sports.Setup(r => r.Get(It.IsAny<string>())).Throws(new SportNotFoundException());
            Team modified = testService.Modify(testDto);
        }

        [TestMethod]
        public void GetAllTeamsTest() {
            GrantFollowerPermissions();
            teams.Setup(r => r.GetAll()).Returns(new List<Team>() { testTeam, testTeam, testTeam });
            ICollection<Team> allTeams = testService.GetAllTeams();
            teams.Verify(r => r.GetAll(), Times.Once);
            Assert.AreEqual(3, allTeams.Count);
        }

        [TestMethod]
        public void GetAllTeamsNotLoggedTest() {
            LogOut();
            testService.GetAllTeams();
        }

        private void GrantAdminPermissions() {
            auth.Setup(r => r.IsLoggedIn()).Returns(true);
            auth.Setup(r => r.HasAdminPermissions()).Returns(true);
        }
        private void GrantFollowerPermissions() {
            auth.Setup(r => r.IsLoggedIn()).Returns(true);
            auth.Setup(r => r.HasAdminPermissions()).Returns(false);
        }

        private void LogOut() {
            auth.Setup(r => r.IsLoggedIn()).Returns(false);
        }
    }
}
