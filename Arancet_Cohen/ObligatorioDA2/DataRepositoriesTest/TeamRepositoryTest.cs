using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;
using DataAccess;
using DataRepositoryInterfaces;
using DataRepositories;
using System.Collections.Generic;
using BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using RepositoryInterface;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace DataRepositoriesTest
{
    [TestClass]
    public class TeamRepositoryTest
    {
        private IRepository<Team> teamsStorage;

        [TestInitialize]
        public void TestInitialize(){
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "TeamRepository")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            teamsStorage = new TeamRepository(context);
            ClearDataBase(context);        
        }

        private void ClearDataBase(DatabaseConnection context)
        { 
                foreach (TeamEntity team in context.Teams) {
                    context.Teams.Remove(team);
                }
                context.SaveChanges();  
        }

        [TestMethod]
        public void NoTeamsTest(){
            bool noTeams = teamsStorage.IsEmpty();
            Assert.IsTrue(noTeams);
        }

        [TestMethod]
        public void AddTeamTest(){
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            Assert.AreEqual(1,teamsStorage.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyExistsException))]
        public void AddAlreadyExistentTeamTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            teamsStorage.Add(team.Object);
        }

        [TestMethod]
        public void GetTeamTest()
        {
            ITeamRepository specific = (ITeamRepository)teamsStorage;
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            Team teamInDb = specific.GetTeamByName("DreamTeam");
            Assert.AreEqual("DreamTeam", teamInDb.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetNotExistentTeamTest() {
            ITeamRepository specific = (ITeamRepository)teamsStorage;
            Team teamInDb = specific.GetTeamByName("DreamTeam");
        }

        [TestMethod]
        public void ExistsTeamTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            bool result = teamsStorage.Exists(team.Object);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesNotExistTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam2.png");
            teamsStorage.Add(team1.Object);
            bool result = teamsStorage.Exists(team2.Object);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            teamsStorage.Delete(team.Object.Id);
            Assert.IsTrue(teamsStorage.IsEmpty());
        }

        [TestMethod]
        public void DeleteByIdTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            teamsStorage.Delete(team.Object.Id);
            Assert.IsTrue(teamsStorage.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void DeleteNotExistentTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam2.png");
            teamsStorage.Add(team1.Object);
            teamsStorage.Delete(team2.Object.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void DeleteByIdNotExistentTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam2.png");
            teamsStorage.Add(team1.Object);
            teamsStorage.Delete(team2.Object.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void DeleteNotExistentByIdTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam2.png");
            teamsStorage.Add(team1.Object);
            teamsStorage.Delete(team2.Object.Id);
        }

        [TestMethod]
        public void ModifyTeamTest(){
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            team.Object.Photo = "NewDreamTeam.png";
            teamsStorage.Modify(team.Object);
            Team editedTeam = teamsStorage.Get(team.Object.Id);
            Assert.AreEqual(team.Object.Photo, editedTeam.Photo);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void ModifyNotExistentTest() {
            Mock<Team> team = new Mock<Team>(1,"DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            team.Object.Name = "TheDream";
            teamsStorage.Modify(team.Object);
        }

        [TestMethod]
        public void ClearTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam1", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam.png");
            Mock<Team> team3 = new Mock<Team>(3, "DreamTeam3", "MyResources/DreamTeam.png");

            teamsStorage.Add(team1.Object);
            teamsStorage.Add(team2.Object);
            teamsStorage.Add(team3.Object);

            teamsStorage.Clear();
            Assert.IsTrue(teamsStorage.IsEmpty());
        }

        [TestMethod]
        public void GetAllTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam1", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam.png");
            Mock<Team> team3 = new Mock<Team>(3, "DreamTeam3", "MyResources/DreamTeam.png");
            team1.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "DreamTeam1");
            team2.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "DreamTeam2");
            team3.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Name == "DreamTeam3");

            teamsStorage.Add(team1.Object);
            teamsStorage.Add(team2.Object);
            teamsStorage.Add(team3.Object);

            ICollection<Team> teams = teamsStorage.GetAll();
            
            Assert.AreEqual(3, teams.Count);
        }

        [TestMethod]
        public void GetByIdTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add(team.Object);
            Team teamInDb = teamsStorage.Get(1);
            Assert.AreEqual("DreamTeam", teamInDb.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetByIdNotExistentTeamTest() {
            Team teamInDb = teamsStorage.Get(1);
        }

    }
}