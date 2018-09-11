using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using System.Collections.Generic;

namespace BusinessLogicTest
{
    [TestClass]
    public class SportTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            Sport sport = new Sport();
            Assert.IsNotNull(sport);
        }

        [TestMethod]
        public void ConstructorWithParametersTest()
        {
            string name = "Football";
            Sport sport = new Sport(name);
            Assert.IsNotNull(sport);
        }

        [TestMethod]
        public void AddTeamTest()
        {
            string name = "Football";
            Sport sport = new Sport(name);

            Team team = new Team("TheTeam", "");
            sport.Add(team);

            Assert.IsTrue(sport.HasTeam(team));
        }

        [TestMethod]
        public void RemoveTeamTest()
        {
            string name = "Football";
            Sport sport = new Sport(name);

            Team team = new Team("TheTeam", "");
            sport.Add(team);
            sport.Remove(team);

            Assert.IsFalse(sport.HasTeam(team));
        }

        [TestMethod]
        public void GetTeamsTest()
        {
            string name = "Football";
            Sport sport = new Sport(name);

            Team team1 = new Team("Team1", "");
            Team team2 = new Team("Team2", "");
            Team team3 = new Team("Team3", "");

            sport.Add(team1);
            sport.Add(team2);
            sport.Add(team3);

            ICollection<Team> teams = sport.GetTeams();

            Assert.IsTrue(teams.Contains(team1));
            Assert.IsTrue(teams.Contains(team2));
            Assert.IsTrue(teams.Contains(team3));
        }

        [TestMethod]
        public void AddTeamsTest()
        {
            string name = "Football";
            Sport sport = new Sport(name);

            Team previousTeam = new Team("prevTeam", "");
            sport.Add(previousTeam);

            ICollection<Team> teams = new List<Team>();
            Team team1 = new Team("Team1", "");
            Team team2 = new Team("Team2", "");
            Team team3 = new Team("Team3", "");
            teams.Add(team1);
            teams.Add(team2);
            teams.Add(team3);

            sport.Add(teams);

            Assert.IsTrue(sport.HasTeam(previousTeam));
            Assert.IsTrue(sport.HasTeam(team1));
            Assert.IsTrue(sport.HasTeam(team2));
            Assert.IsTrue(sport.HasTeam(team3));
        }

        [TestMethod]
        public void EqualsTest()
        {
            Sport aSport = new Sport("SportA");
            Sport sameSport = new Sport("SportA");
            Assert.AreEqual(aSport, sameSport);
        }

        [TestMethod]
        public void NotEqualsTest()
        {
            Sport aSport = new Sport("SportA");
            Sport differentSport = new Sport("SportB");
            Assert.AreNotEqual(aSport, differentSport);
        }
    }
}
