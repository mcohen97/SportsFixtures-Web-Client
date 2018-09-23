using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using BusinessLogicExceptions;

namespace BusinessLogicTest
{
    [TestClass]
    public class TeamTest
    {
        private Team testTeam;

        [TestInitialize]
        public void TestInitialize(){
            int id = 1;
            string name = "TheTeam";
            string photo = "myresource/theteam.png";
            testTeam = new Team(id, name, photo);
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
            Team aTeam = new Team(1, "TeamA", "somePath");
            Team sameTeam = new Team(1, "TeamA", "");
            Assert.AreEqual(aTeam, sameTeam);
        }

        [TestMethod]
        public void EqualsByIdTest()
        {
            Team aTeam = new Team(1, "TeamA", "somePath");
            Team differentTeam = new Team(1, "TeamB", "");
            Assert.AreEqual(aTeam, differentTeam);
        }

        [TestMethod]
        public void EqualsByNameTest()
        {
            Team aTeam = new Team(1, "TeamA", "somePath");
            Team differentTeam = new Team(2, "TeamA", "");
            Assert.AreEqual(aTeam, differentTeam);
        }

        [TestMethod]
        public void NotEqualsTest()
        {
            Team aTeam = new Team(1, "TeamA", "somePath");
            Team differentTeam = new Team(2, "TeamB", "");
            Assert.AreNotEqual(aTeam, differentTeam);
        }

        //Exceptions

        [TestMethod]
        [ExpectedException(typeof(InvalidCommentaryDataException))]
        public void EmptyNameConstructorTest()
        {
            Team team = new Team(1, "","photo");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommentaryDataException))]
        public void SetEmptyNameTest()
        {
            Team team = new Team(1, "name","photo");
            team.Name = "";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommentaryDataException))]
        public void SetNullNameTest()
        {
            Team team = new Team(1, "name","photo");
            team.Name = null;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommentaryDataException))]
        public void SetNullPhotoTest()
        {
            Team team = new Team(1, "name","photo");
            team.Photo = null;
        }
    }
}
