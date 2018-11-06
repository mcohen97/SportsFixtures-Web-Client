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
            TeamDto fetched = testService.GetTeam(1);
            Assert.AreEqual(testTeam.Name,fetched.name);
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

            TeamDto added =testService.AddTeam(testDto);

            auth.VerifyAll();
            sports.VerifyAll();
            teams.VerifyAll();
            Assert.AreEqual(testTeam.Id, added.id);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void AddTeamInvalidDataTest()
        {
            GrantAdminPermissions();
            sports.Setup(r => r.Get(testSport.Name)).Returns(testSport);
            teams.Setup(r => r.Add(testTeam)).Returns(testTeam);

            testDto.name = null;
            testService.AddTeam(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void AddTeamAlreadyExistentTest()
        {
            GrantAdminPermissions();
            sports.Setup(r => r.Get(testSport.Name)).Returns(testSport);
            teams.Setup(r => r.Add(testTeam)).Throws(new TeamAlreadyExistsException());
            testService.AddTeam(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void AddTeamNotExistentSportTest() {
            GrantAdminPermissions();
            sports.Setup(r => r.Get(It.IsAny<string>())).Throws(new SportNotFoundException());
            teams.Setup(r => r.Add(testTeam)).Returns(testTeam);

            testService.AddTeam(testDto);    
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]
        public void AddTeamNotLoggedException() {
            LogOut();
            testService.AddTeam(testDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NoPermissionsException))]
        public void AddTeamNotAuthorizedException()
        {
            GrantFollowerPermissions();
            testService.AddTeam(testDto);
        }

        [TestMethod]
        public void ModifyTeamTest() {
            GrantAdminPermissions();
            teams.Setup(r => r.Get(It.IsAny<int>())).Returns(testTeam);
            testDto.name = "Manchester United";
            TeamDto modified = testService.Modify(testDto);
            teams.Verify(r => r.Modify(It.IsAny<Team>()), Times.Once);
            Assert.AreEqual(testDto.name, modified.name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void ModifyTeamNotFoundTest()
        {
            GrantAdminPermissions();
            teams.Setup(r => r.Get(It.IsAny<int>())).Throws(new TeamNotFoundException());
            sports.Setup(r => r.Get(It.IsAny<string>())).Throws(new SportNotFoundException());
            testService.Modify(testDto);
        }

        [TestMethod]
        public void GetAllTeamsTest() {
            GrantFollowerPermissions();
            teams.Setup(r => r.GetAll()).Returns(new List<Team>() { testTeam, testTeam, testTeam });
            ICollection<TeamDto> allTeams = testService.GetAllTeams();
            teams.Verify(r => r.GetAll(), Times.Once);
            Assert.AreEqual(3, allTeams.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]

        public void GetAllTeamsNotLoggedTest() {
            LogOut();
            testService.GetAllTeams();
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]

        public void GetAllTeamsNoDataAccessTest()
        {
            GrantFollowerPermissions();
            teams.Setup(r => r.GetAll()).Throws(new DataInaccessibleException());
            testService.GetAllTeams();
        }

        [TestMethod]
        public void DeleteTest() {
            GrantAdminPermissions();
            testService.DeleteTeam(2);
            teams.Verify(r => r.Delete(2), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void DeleteNotFoundException() {
            GrantAdminPermissions();
            teams.Setup(r => r.Delete(2)).Throws(new TeamNotFoundException());
            testService.DeleteTeam(2);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void DeleteNoDataAccessTest() {
            GrantAdminPermissions();
            teams.Setup(r => r.Delete(It.IsAny<int>())).Throws(new DataInaccessibleException());
            testService.DeleteTeam(2);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]
        public void DeleteNotLoggedTest() {
            LogOut();
            testService.DeleteTeam(2);
        }

        [TestMethod]
        [ExpectedException(typeof(NoPermissionsException))]
        public void DeleteNoPermissionsTest() {
            GrantFollowerPermissions();
            testService.DeleteTeam(2);
        }


        [TestMethod]
        public void GetSportTeamsTest()
        {
            GrantFollowerPermissions();
            teams.Setup(r => r.GetTeams(testSport.Name)).Returns(new List<Team>() { testTeam, testTeam, testTeam });
            ICollection<TeamDto> result = testService.GetSportTeams(testSport.Name);
            auth.Verify(r => r.IsLoggedIn(), Times.Once);
            auth.Verify(r => r.HasAdminPermissions(), Times.Never);
            teams.Verify(r => r.GetTeams(testSport.Name), Times.Once);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetSportTeamsNotFoundTest()
        {
            GrantFollowerPermissions();
            teams.Setup(r => r.GetTeams(testSport.Name)).Throws(new SportNotFoundException());
            testService.GetSportTeams(testSport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetSportTeamsNoDataAccessTest()
        {
            GrantFollowerPermissions();
            teams.Setup(r => r.GetTeams(testSport.Name)).Throws(new DataInaccessibleException());
            testService.GetSportTeams(testSport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(NotAuthenticatedException))]
        public void GetSportTeamsWhenNotLoggedTest()
        {
            LogOut();
            testService.GetSportTeams(testSport.Name);
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
