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
        private ISportRepository sportsStorage;

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
            sportsStorage = new SportRepository(context);
            teamsStorage = new TeamRepository(context);
        }

        private void ClearDataBase()
        {
            teamsStorage.Clear();
            sportsStorage.Clear();
        }

        [TestMethod]
        public void NoTeamsTest(){
            bool noTeams = teamsStorage.IsEmpty();
            Assert.IsTrue(noTeams);
        }

        [TestMethod]
        public void AddTeamTest(){
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            teamsStorage.Add(team.Object);
            Assert.AreEqual(1,teamsStorage.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamAlreadyExistsException))]
        public void AddAlreadyExistentTeamTest() {
            Team team = new Team(1, "DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            teamsStorage.Add(team);
            SetUpRepository();
            teamsStorage.Add(team);
        }

        [TestMethod]
        public void GetTeamTest()
        {
            ITeamRepository specific = (ITeamRepository)teamsStorage;
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            teamsStorage.Add(team.Object);
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
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            teamsStorage.Add(team.Object);
            bool result = teamsStorage.Exists("Soccer",team.Object.Name);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesNotExistTest() {
            Team team1 = new Team(1, "DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            Team team2 = new Team(2, "DreamTeam2", "MyResources/DreamTeam2.png", new Sport("Soccer"));
            teamsStorage.Add(team1);
            bool result = teamsStorage.Exists("Basketball",team1.Name);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteTest() {
            Team team = new Team(1, "DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            teamsStorage.Add(team);
            teamsStorage.Delete("Soccer",team.Name);
            Assert.IsTrue(teamsStorage.IsEmpty());
        }


        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void DeleteNotExistentTest() {
            Team team1 = new Team(1, "DreamTeam", "MyResources/DreamTeam.png",new Sport("Soccer"));
            Team team2 = new Team(2, "DreamTeam2", "MyResources/DreamTeam2.png", new Sport("Soccer"));
            teamsStorage.Add(team1);
            teamsStorage.Delete(team2.Sport.Name, team2.Name);
        }

        [TestMethod]
        public void ModifyTeamTest(){
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            teamsStorage.Add(team.Object);
            team.Object.Photo = "NewDreamTeam.png";
            SetUpRepository();
            teamsStorage.Modify(team.Object);
            Team editedTeam = teamsStorage.Get("Soccer",team.Object.Name);
            Assert.AreEqual(team.Object.Photo, editedTeam.Photo);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void ModifyNotExistentTest() {
            Mock<Team> team = new Mock<Team>(1,"DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            teamsStorage.Add(team.Object);
            SetUpRepository();
            teamsStorage.Modify(team.Object);
        }

        [TestMethod]
        public void ClearTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam1", "MyResources/DreamTeam.png", new Sport("Soccer"));
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam.png", new Sport("Soccer"));
            Mock<Team> team3 = new Mock<Team>(3, "DreamTeam3", "MyResources/DreamTeam.png", new Sport("Soccer"));

            teamsStorage.Add(team1.Object);
            teamsStorage.Add(team2.Object);
            teamsStorage.Add(team3.Object);

            teamsStorage.Clear();
            Assert.IsTrue(teamsStorage.IsEmpty());
        }

        [TestMethod]
        public void GetAllTest() {
            Mock<Team> team1 = new Mock<Team>(1, "DreamTeam1", "MyResources/DreamTeam.png", new Sport("Soccer"));
            Mock<Team> team2 = new Mock<Team>(2, "DreamTeam2", "MyResources/DreamTeam.png", new Sport("Soccer"));
            Mock<Team> team3 = new Mock<Team>(3, "DreamTeam3", "MyResources/DreamTeam.png", new Sport("Soccer"));
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
            Mock<Team> team = new Mock<Team>(1, "DreamTeam", "MyResources/DreamTeam.png", new Sport("Soccer"));
            teamsStorage.Add(team.Object);
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
                teamsStorage.Add(created);
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