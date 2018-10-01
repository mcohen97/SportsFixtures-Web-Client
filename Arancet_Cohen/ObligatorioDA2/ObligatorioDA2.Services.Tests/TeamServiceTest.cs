using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ObligatorioDA2.Services.Tests
{
    public class TeamServiceTest
    {

        private ITeamService teamService;
        private Mock<ITeamRepository> mockTeams;
        private Mock<ISportRepository> mockSports;
        private Team testTeam;

        [TestMethod]
        public void SetUp() {
            mockTeams = new Mock<ITeamRepository>();
            teamService = new TeamService(mockTeams.Object);
            testTeam = new Team(1,"aTeam", "aPhoto",new Sport("aSport"));
        }

        [TestMethod]
        public void AddTeamTest() {
            Sport played = new Sport("Soccer");
            teamService.AddTeam(played, testTeam);
            mockSports.Verify(s => s.Exists(It.IsAny<Sport>()));
            mockTeams.Verify(t => t.Add( It.IsAny<Team>()));
        }

    }
}
