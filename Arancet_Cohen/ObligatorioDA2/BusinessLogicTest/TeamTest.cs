using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;

namespace BusinessLogicTest
{
    [TestClass]
    public class TeamTest
    {
        [TestMethod]
        public void ConstructorTeamTest()
        {
            Team team = new Team();
            Assert.IsNotNull(team);
        }

        [TestMethod]
        public void ConstructorParametersTeamTest()
        {
            string name = "TheTeam";
            string photo = "myresource/theteam.png";
            Team team = new Team(name, photo);
            Assert.IsNotNull(team);
        }

        [TestMethod]
        public void GetNameTest()
        {
            string name = "TheTeam";
            string photo = "myresourcce/theteam.png";
            Team team = new Team(name, photo);
            Assert.AreEqual(name, team.Name);
        }

        [TestMethod]
        public void SetNameTest()
        {
            string name = "TheTeam";
            Team team = new Team();
            team.Name = name;
            Assert.AreEqual(name, team.Name);
        }

        [TestMethod]
        public void GetPhotoTest()
        {
            string name = "TheTeam";
            string photo = "myresource/theteam.png";
            Team team = new Team(name, photo);
            Assert.AreEqual(photo, team.Photo);
        }

        [TestMethod]
        public void SetPhotoTest()
        {
            string photo = "myresource/theteam.png";
            Team team = new Team();
            team.Photo = photo;
            Assert.AreEqual(photo, team.Photo);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Team aTeam = new Team("TeamA", "somePath");
            Team sameTeam = new Team("TeamA", "");
            Assert.AreEqual(aTeam, sameTeam);
        }

        [TestMethod]
        public void NotEqualsTest()
        {
            Team aTeam = new Team("TeamA", "somePath");
            Team differentTeam = new Team("TeamB", "");
            Assert.AreNotEqual(aTeam, differentTeam);
        }
    }
}
