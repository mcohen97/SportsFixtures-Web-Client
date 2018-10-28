using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ObligatorioDA2.BusinessLogic.Test
{
    [TestClass]
    public class EncounterFactoryTest
    {
        private EncounterFactory testFactory;

        private Team teamA;
        private Team teamB;
        private Team teamC;
        private Sport sport;
        private DateTime date;
        private Encounter match;
        private Commentary commentary1;
        private Commentary commentary2;
        private Commentary commentary3;

        [TestInitialize]
        public void SetUp() {
            testFactory = new EncounterFactory();

            sport = new Sport("Soccer", true);
            teamA = new Team(1, "TeamA", "Photo/A", sport);
            teamB = new Team(2, "TeamB", "Photo/B", sport);
            teamC = new Team(2, "TeamC", "Photo/C", sport);
            date = new DateTime(2019, 1, 25, 13, 30, 0);
            User commentarist = CreateUser();
            commentary1 = new Commentary(1, "Commentary 1", commentarist);
            commentary2 = new Commentary(2, "Commentary 2", commentarist);
            commentary3 = new Commentary(3, "Commentary 3", commentarist);
        }

        private User CreateUser()
        {
            UserId identity = new UserId()
            {
                Name = "aName",
                Surname = "aSurname",
                UserName = "aUsername",
                Password = "aPassword",
                Email = "anEmail@aDomain.com"
            };
            User toReturn = new User(identity, false);
            return toReturn;
        }

        [TestMethod]
        public void BuildMatch3ArgumentsTest() {
            Encounter testMatch = testFactory.CreateEncounter(new List<Team>() { teamA, teamB }, date, sport);
            Match downcast = testMatch as Match;
            Assert.IsNotNull(downcast);
        }

        [TestMethod]
        public void BuildCompetition3ArgumentsTest() {
            ChangeSport();
            Encounter testMatch = testFactory.CreateEncounter(new List<Team>() { teamA, teamB }, date, sport);
            Competition downcast = testMatch as Competition;
            Assert.IsNotNull(downcast);
        }

        private void ChangeSport() {
            sport = new Sport("Archery", false);
            teamA = new Team(1, "TeamA", "Photo/A", sport);
            teamB = new Team(2, "TeamB", "Photo/B", sport);
            teamC = new Team(2, "TeamC", "Photo/C", sport);
        }

    }
}
