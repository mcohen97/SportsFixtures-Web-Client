using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Moq;

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

        /*[TestMethod]
        public void EqualsByIdTest()
        {
            Team aTeam = new Team(1, "TeamA", "somePath");
            Team differentTeam = new Team(1, "TeamB", "");
            Assert.AreEqual(aTeam, differentTeam);
        }*/

        [TestMethod]
        public void NotEqualsTest()
        {
            Team aTeam = new Team(1, "TeamA", "somePath", new Sport("aSport"));
            Team differentTeam = new Team(2, "TeamB", "", new Sport("aSport"));
            Assert.AreNotEqual(aTeam, differentTeam);
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
    }
}
