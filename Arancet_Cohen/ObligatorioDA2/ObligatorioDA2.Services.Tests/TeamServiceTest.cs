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
        private ITeamRepository teamStorage;
        private ISportRepository sportStorage;
        private Team testTeam;

        [TestMethod]
        public void SetUp() {
        }

        [TestMethod]
        public void AddTeamTest() {
            Sport played = new Sport("Soccer");
            teamService.AddTeam(played, testTeam);
          
        }

    }
}
