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
        private EncounterModelFactory factory;

        [TestInitialize]
        public void SetUp() {
            Sport sport = new Sport("Soccer", true);
            Team teamA = new Team(1, "teamA", "photo", sport);
            Team teamB = new Team(2, "teamB", "photo", sport);
            Team teamC = new Team(3, "teamC", "photo", sport);
            testMatch = new Match(1, new List<Team>() { teamA, teamB }, DateTime.Now.AddDays(1), sport);
            ChangeSport(ref sport,ref teamA,ref teamB,ref teamC);
            testCompetition = new Competition(2, new List<Team>() { teamA, teamB, teamC }, DateTime.Now.AddDays(2), sport);
            SetResult(testMatch);
            SetResult(testCompetition);
            factory = new EncounterModelFactory();
        }

        private void ChangeSport(ref Sport sport, ref Team teamA, ref Team teamB, ref Team teamC)
        {
            sport = new Sport("Golf", false);
            teamA = new Team(1, "teamA", "photo", sport);
            teamB = new Team(2, "teamB", "photo", sport);
            teamC = new Team(3, "teamC", "photo", sport);
        }

        private void SetResult(Encounter testMatch)
        {
            Result res = new Result();
            int pos = 1;
            foreach (Team t in testMatch.GetParticipants()) {
                res.Add(t, pos);
                pos++;
            }
            testMatch.Result=res;
        }

        [TestMethod]
        public void CompetitionModelOutTest()
        {
            EncounterModelOut modelOut = factory.CreateModelOut(testCompetition);
            CompetitionModelOut competition = modelOut as CompetitionModelOut;
            Assert.IsNotNull(competition);
            Assert.AreEqual(competition.Team_Position.Count, 3);
        }

        [TestMethod]
        public void MatchModelOutTest()
        {
            EncounterModelOut modelOut = factory.CreateModelOut(testMatch);
            MatchModelOut match = modelOut as MatchModelOut;
            Assert.IsNotNull(match);
            Assert.IsFalse(match.HasResult);
            Assert.IsTrue(match.HasWinner);
        }
    }
}
