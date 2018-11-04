using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;

namespace ObligatorioDA2.Services.Tests
{
    public class SportServiceTest
    {
        private ISportService serviceToTest;
        private Mock<ISportRepository> sportsStorage;
        private Mock<ITeamRepository> teamsStorage;
        private Mock<IAuthenticationService> authentication;
        private Sport testSport;

        [TestInitialize]
        public void SetUp() {
            sportsStorage = new Mock<ISportRepository>();
            teamsStorage = new Mock<ITeamRepository>();
            authentication = new Mock<IAuthenticationService>();
            serviceToTest = new SportService(sportsStorage.Object, teamsStorage.Object, authentication.Object);
            testSport = new Sport("Sport", true);
        }

        [TestMethod]
        public void GetAllTest() {
            GrantFollowerPermissions();
            sportsStorage.Setup(r => r.GetAll()).Returns(new List<Sport>() { testSport, testSport, testSport });
            ICollection<Sport> result= serviceToTest.GetAllSports();
            authentication.Verify(r => r.IsLoggedIn(), Times.Once);
            authentication.Verify(r => r.HasAdminPermissions(), Times.Never);
            sportsStorage.Verify(r => r.GetAll(), Times.Once);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetAllNoAuthTest() {
            LogOut();
            serviceToTest.GetAllSports();
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetAllNoDataAccessTest()
        {
            GrantFollowerPermissions();
            sportsStorage.Setup(r => r.GetAll()).Throws(new DataInaccessibleException());
            serviceToTest.GetAllSports();
        }

        [TestMethod]
        public void GetTeamTest() {
            GrantFollowerPermissions();
            sportsStorage.Setup(r => r.Get(testSport.Name)).Returns(testSport);
            Sport result = serviceToTest.GetSport(testSport.Name);
            authentication.Verify(r => r.IsLoggedIn(), Times.Once);
            authentication.Verify(r => r.HasAdminPermissions(), Times.Never);
            sportsStorage.Verify(r => r.Get(testSport.Name), Times.Once);
            Assert.AreEqual(testSport.Name, result.Name);
            Assert.AreEqual(testSport.IsTwoTeams, result.IsTwoTeams);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetTeamNotFoundTest() {
            GrantFollowerPermissions();
            sportsStorage.Setup(r => r.Get(testSport.Name)).Throws(new SportNotFoundException());
            serviceToTest.GetSport(testSport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetTeamNoDataAccess() {
            GrantFollowerPermissions();
            sportsStorage.Setup(r => r.Get(testSport.Name)).Throws(new DataInaccessibleException());
            serviceToTest.GetSport(testSport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceException))]
        public void GetSportNoAuthTest()
        {
            LogOut();
            serviceToTest.GetSport(testSport.Name);
        }


        private void GrantAdminPermissions()
        {
            authentication.Setup(r => r.IsLoggedIn()).Returns(true);
            authentication.Setup(r => r.HasAdminPermissions()).Returns(true);
        }
        private void GrantFollowerPermissions()
        {
            authentication.Setup(r => r.IsLoggedIn()).Returns(true);
            authentication.Setup(r => r.HasAdminPermissions()).Returns(false);
        }

        private void LogOut()
        {
            authentication.Setup(r => r.IsLoggedIn()).Returns(false);
        }
    }
}
