using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Moq;
using System.Collections.Generic;

namespace BusinessLogicTest
{
    [TestClass]
    public class TeamTest
    {
        private Team testTeam;
        private Sport aSport;

        [TestInitialize]
        public void TestInitialize(){
            int id = 1;
            string name = "TheTeam";
            string photo = "myresource/theteam.png";
            aSport = new Mock<Sport>("TheSport").Object;
            testTeam = new Team(id, name, photo, aSport);
        }

        [TestMethod]
        public void ConstructorTeamTest()
        {
            Assert.IsNotNull(testTeam);
        }
        
        [TestMethod]
        public void GetIdTest()
        {
            int id = 1;
            Assert.AreEqual(id, testTeam.Id);
        }

        [TestMethod]
        public void SetIdTest()
        {
            int newId = 2;
            testTeam.Id = newId;
            Assert.AreEqual(newId, testTeam.Id);
        }

        [TestMethod]
        public void GetNameTest()
        {
            string name = "TheTeam";
            Assert.AreEqual(name, testTeam.Name);
        }

        [TestMethod]
        public void SetNameTest()
        {
            string newName = "TheNewTeam";
            testTeam.Name = newName;
            Assert.AreEqual(newName, testTeam.Name);
        }

        [TestMethod]
        public void GetSportTest()
        {
            Assert.AreEqual(testTeam.Sport, aSport);
        }

        [TestMethod]
        public void SetSportTest()
        {
            Sport sport = new Mock<Sport>("TheNewSport").Object;
            testTeam.Sport = sport;
            Assert.AreEqual(sport, testTeam.Sport);
        }

        [TestMethod]
        public void GetPhotoTest()
        {
            string photo = "myresource/theteam.png";
            Assert.AreEqual(photo, testTeam.Photo);
        }

        [TestMethod]
        public void SetPhotoTest()
        {
            string newPhoto = "myresource/thenewteam.png";
            testTeam.Photo = newPhoto;
            Assert.AreEqual(newPhoto, testTeam.Photo);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Team aTeam = new Team(1, "TeamA", "somePath",new Sport("aSport"));
            Team sameTeam = new Team(1, "TeamA", "", new Sport("aSport"));
            Assert.AreEqual(aTeam, sameTeam);
        }

        [TestMethod]
        public void NotEqualsTest()
        {
            Team aTeam = new Team(1, "TeamA", "somePath", new Sport("aSport"));
            Team differentTeam = new Team(2, "TeamB", "", new Sport("aSport"));
            Assert.AreNotEqual(aTeam, differentTeam);
        }

        [TestMethod]
        public void NotEqualsDifferentSport() {
            Team aTeam = new Team(1, "TeamA", "somePath", new Sport("aSport"));
            Team differentTeam = new Team(2, "TeamA", "", new Sport("anotherSport"));
            Assert.AreNotEqual(aTeam, differentTeam);
        }

        [TestMethod]
        public void NotEqualsDifferentTypeTest()
        {
            Team aTeam = new Team(1, "TeamA", "somePath", new Sport("aSport"));
            Assert.IsFalse(aTeam.Equals("this should be false"));
        }

        [TestMethod]
        public void GetHashCodeTest() {
            Team aTeam = new Team(1, "TeamA", "somePath", new Sport("aSport"));
            int hashCode= 539060726 + EqualityComparer<string>.Default.GetHashCode("TeamA");
            Assert.AreEqual(hashCode, aTeam.GetHashCode());
        }

        //Exceptions

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void EmptyNameConstructorTest()
        {
            Team team = new Team(1, "","photo", new Sport("aSport"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void SetEmptyNameTest()
        {
            Team team = new Team(1, "name","photo", new Sport("aSport"));
            team.Name = "";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void SetNullNameTest()
        {
            Team team = new Team(1, "name","photo", new Sport("aSport"));
            team.Name = null;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTeamDataException))]
        public void SetNullPhotoTest()
        {
            Team team = new Team(1, "name","photo", new Sport("aSport"));
            team.Photo = null;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void SetNullSportTest() {
            Team team = new Team(1, "name", "photo", null);
        }
    }
}
