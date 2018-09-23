using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using System.Collections.Generic;
using Moq;
using BusinessLogic.Exceptions;

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
            Sport sport = new Sport();

            Mock<Team> team = new Mock<Team>("ATeam", "");
            team.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "ATeam");
            sport.Add(team.Object);

            Assert.IsTrue(sport.HasTeam(team.Object));
        }

        [TestMethod]
        public void RemoveTeamTest()
        {
            string name = "Football";
            Sport sport = new Sport(name);

            Mock<Team> team = new Mock<Team>();
            sport.Add(team.Object);
            sport.Remove(team.Object);

            Assert.IsFalse(sport.HasTeam(team.Object));
        }

        [TestMethod]
        public void GetTeamsTest()
        {
            string name = "Football";
            Sport sport = new Sport(name);

            Mock<Team> team1 = new Mock<Team>("team1", "");
            Mock<Team> team2 = new Mock<Team>("team2", "");
            Mock<Team> team3 = new Mock<Team>("team3", "");
            team1.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "team1");
            team2.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "team2");
            team3.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "team3");

            sport.Add(team1.Object);
            sport.Add(team2.Object);
            sport.Add(team3.Object);
            ICollection<Team> teams = sport.GetTeams();

            Assert.IsTrue(teams.Contains(team1.Object));
            Assert.IsTrue(teams.Contains(team2.Object));
            Assert.IsTrue(teams.Contains(team3.Object));
        }

        [TestMethod]
        public void AddTeamsTest()
        {
            Sport sport = new Sport();
            Mock<Team> previousTeam = new Mock<Team>("prevTeam", "");
            Mock<Team> team1 = new Mock<Team>("team1", "");
            Mock<Team> team2 = new Mock<Team>("team2", "");
            Mock<Team> team3 = new Mock<Team>("team3", "");
            previousTeam.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "prevTeam");
            team1.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "team1");
            team2.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "team2");
            team3.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "team3");

            ICollection<Team> teams = new List<Team> { team1.Object, team2.Object, team3.Object };

            sport.Add(previousTeam.Object);
            sport.Add(teams);

            Assert.IsTrue(sport.HasTeam(previousTeam.Object));
            Assert.IsTrue(sport.HasTeam(team1.Object));
            Assert.IsTrue(sport.HasTeam(team2.Object));
            Assert.IsTrue(sport.HasTeam(team3.Object));
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

        [TestMethod]
        public void SetNameTest()
        {
            Sport aSport = new Sport("SportA");
            string newName = "NewName";
            aSport.Name = newName;
            Assert.AreEqual(newName,aSport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void SetEmptyNameTest()
        {
            Sport sport = new Sport("Name");
            sport.Name = "";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void SetNullNameTest()
        {
            Sport sport = new Sport("Name");
            sport.Name = "";
        }
    }
}
