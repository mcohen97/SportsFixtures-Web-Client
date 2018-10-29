using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class SportTableServiceTest
    {
        private SportTableService serviceToTest;
        private Mock<ISportRepository> sportsStorage;
        private Mock<ITeamRepository> teamsStorage;

        [TestInitialize]
        public void SetUp() {
            serviceToTest = new SportTableService();
            sportsStorage = new Mock<ISportRepository>();
            teamsStorage = new Mock<ITeamRepository>();
            SetUpSports();
            SetUpTeams();
        }

        private void SetUpSports()
        {
            Sport twoTeamSport = new Sport("Soccer", true);
            Sport multipleTeamSport = new Sport("Archery", false);
            sportsStorage.Setup(r => r.Get("Soccer")).Returns(twoTeamSport);
            sportsStorage.Setup(r => r.Get("Archery")).Returns(multipleTeamSport);
            sportsStorage.Setup(r => r.Get(It.Is<string>(s => !s.Equals("Soccer") && !s.Equals("Archery"))))
                .Throws(new SportNotFoundException());
        }

        private void SetUpTeams()
        {
            Sport twoTeamSport = new Sport("Soccer", true);
            Team teamA = new Team("teamA", "photoA", twoTeamSport);
            Team teamB = new Team("teamA", "photoA", twoTeamSport);
            Team teamC = new Team("teamA", "photoA", twoTeamSport);

            Sport multipleTeamSport = new Sport("Archery", false);
            Team teamD = new Team("teamD", "photoD", multipleTeamSport);
            Team teamE = new Team("teamE", "photoE", multipleTeamSport);
            Team teamF = new Team("teamF", "photoF", multipleTeamSport);

            teamsStorage.Setup(r => r.GetTeams("Soccer")).Returns(new List<Team>() { teamA, teamB, teamC });
            teamsStorage.Setup(r => r.GetTeams("Archery")).Returns(new List<Team>() { teamD, teamE, teamF });
        }
    }
}
