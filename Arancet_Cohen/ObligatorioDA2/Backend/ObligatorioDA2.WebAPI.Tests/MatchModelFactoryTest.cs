using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.WebAPI.Models;
using System;
using System.Collections.Generic;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class MatchModelFactoryTest
    {
        private Encounter testMatch;
        private Encounter testCompetition;
        private MatchModelFactory factory;

        [TestMethod]
        public void StartUp() {
            Sport sport = new Sport("Soccer", true);
            Team teamA = new Team(1, "teamA", "photo", sport);
            Team teamB = new Team(2, "teamB", "photo", sport);
            Team teamC = new Team(3, "teamC", "photo", sport);
            testMatch = new Match(1, new List<Team>() { teamA, teamB }, DateTime.Now.AddDays(1), sport);
            testCompetition = new Match(2, new List<Team>() { teamA,teamB ,teamC }, DateTime.Now.AddDays(2), sport);
            factory = new MatchModelFactory();
        }

        
    }
}
