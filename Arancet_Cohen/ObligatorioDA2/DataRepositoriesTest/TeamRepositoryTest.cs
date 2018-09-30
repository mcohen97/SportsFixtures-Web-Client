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
        private ITeamRepository teamsStorage;

        [TestInitialize]
        public void TestInitialize(){
            SetUpRepository();
            ClearDataBase();        
        }
        private void SetUpRepository() {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
               .UseInMemoryDatabase(databaseName: "TeamRepository")
               .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            teamsStorage = new TeamRepository(context);
        }

        private void ClearDataBase()
        {
            teamsStorage.Clear();
        }

        [TestMethod]
        public void NoTeamsTest(){
            bool noTeams = teamsStorage.IsEmpty();
            Assert.IsTrue(noTeams);
        }

        [TestMethod]
        public void AddTeamTest(){
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add("Soccer",team.Object);
            Assert.AreEqual(1,teamsStorage.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyExistsException))]
        public void AddAlreadyExistentTeamTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add("Soccer",team.Object);
            SetUpRepository();
            teamsStorage.Add("Soccer",team.Object);
        }

        [TestMethod]
        public void GetTeamTest()
        {
            ITeamRepository specific = (ITeamRepository)teamsStorage;
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add("Soccer",team.Object);
            Team teamInDb = specific.Get("Soccer","DreamTeam");
            Assert.AreEqual("DreamTeam", teamInDb.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetNotExistentTeamTest() {
            Team teamInDb = teamsStorage.Get("Soccer","DreamTeam");
        }

        [TestMethod]
        public void ExistsTeamTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add("Soccer",team.Object);
            bool result = teamsStorage.Exists("Soccer",team.Object.Name);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesNotExistTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam2.png");
            teamsStorage.Add("Soccer",team1.Object);
            bool result = teamsStorage.Exists("Basketball",team1.Object.Name);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add("Soccer",team.Object);
            teamsStorage.Delete("Soccer",team.Object.Name);
            Assert.IsTrue(teamsStorage.IsEmpty());
        }


        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void DeleteNotExistentTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam2.png");
            teamsStorage.Add("Soccer",team1.Object);
            teamsStorage.Delete("Soccer",team2.Object.Name);
        }

        [TestMethod]
        public void ModifyTeamTest(){
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add("Soccer",team.Object);
            team.Object.Photo = "NewDreamTeam.png";
            SetUpRepository();
            teamsStorage.Modify("Soccer",team.Object);
            Team editedTeam = teamsStorage.Get("Soccer",team.Object.Name);
            Assert.AreEqual(team.Object.Photo, editedTeam.Photo);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void ModifyNotExistentTest() {
            Mock<Team> team = new Mock<Team>(1,"DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add("Soccer",team.Object);
            teamsStorage.Modify("Basketball",team.Object);
        }

        [TestMethod]
        public void ClearTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam1", "MyResources/DreamTeam.png");
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam.png");
            Mock<Team> team3 = new Mock<Team>(3, "DreamTeam3", "MyResources/DreamTeam.png");

            teamsStorage.Add("Soccer",team1.Object);
            teamsStorage.Add("Soccer",team2.Object);
            teamsStorage.Add("Soccer",team3.Object);

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

            teamsStorage.Add("Soccer",team1.Object);
            teamsStorage.Add("Soccer",team2.Object);
            teamsStorage.Add("Soccer",team3.Object);

            ICollection<Team> teams = teamsStorage.GetAll();
            
            Assert.AreEqual(3, teams.Count);
        }

        [TestMethod]
        public void GetByIdTest() {
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png");
            teamsStorage.Add("Soccer",team.Object);
            Team teamInDb = teamsStorage.Get("Soccer",team.Object.Name);
            Assert.AreEqual("DreamTeam", teamInDb.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetByIdNotExistentTeamTest() {
            Team teamInDb = teamsStorage.Get("Soccer","Nacional");
        }

        [TestMethod]
        public void GetUserTeamsTest() {
            IUserRepository userRepository = GetUsersRepository();
            User fake = GetFakeUser();
            
            ICollection<Team> teams = GetFakeTeams();
            foreach (Team created in teams) {
                teams.Add(created);
                fake.AddFavourite(created);
            }       
            userRepository.Add(fake);
            ICollection<Team> followedTeams = teamsStorage.GetFollowedTeams(fake.UserName);
            Assert.AreEqual(followedTeams.Count, 3);
        }

        private User GetFakeUser()
        {
            UserId identity = new UserId
            {
                Name = "John",
                Surname = "Doe",
                UserName = "JohnDoe",
                Password = "Password",
                Email = "John@Doe.com"
            };
            return new User(identity, true);
        }

        private IUserRepository GetUsersRepository()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
               .UseInMemoryDatabase(databaseName: "UsersRepository")
               .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            IUserRepository usersStorage = new UserRepository(context);
            return usersStorage;
        }

        private ICollection<Team> GetFakeTeams()
        {
            Sport played = new Sport("Soccer");
            ICollection<Team> teams = new List<Team>() {
                new Team(1, "DreamTeam1", "MyResources/DreamTeam.png",played),
                new Team(2, "DreamTeam2", "MyResources/DreamTeam.png",played),
                new Team(3, "DreamTeam3", "MyResources/DreamTeam.png",played)
            };
            return teams;
        }
        
    }
}