using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Interfaces;

namespace ObligatorioDA2.Services.Tests
{
    public class SportServiceTest
    {
        private ISportService serviceToTest;
        private Mock<ISportRepository> sportsStorage;
        private Mock<ITeamRepository> teamsStorage;
        private Mock<IAuthenticationService> authentication;

        [TestInitialize]
        public void SetUp() {
            sportsStorage = new Mock<ISportRepository>();
            teamsStorage = new Mock<ITeamRepository>();
            serviceToTest = new SportService(sportsStorage.Object, teamsStorage.Object, authentication.Object);
        }
    }
}
