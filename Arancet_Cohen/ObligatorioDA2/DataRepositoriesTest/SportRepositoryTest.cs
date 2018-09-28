using BusinessLogic;
using DataAccess;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;
using RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositoriesTest
{
    [TestClass]
    public class SportRepositoryTest
    {
        private IRepository<Sport> sportStorage;
        private Sport sportA;
        private Sport sportB;
        private ICollection<Team> mockTeamsA;
        private ICollection<Team> mockTeamsB;
        private Team team1;
        private Team team2;
        private Team team3;
        private Team team4;

        [TestInitialize]
        public void TestInitialize()
        {
            DatabaseConnection context = CreateContext();
            CreateTeams();
            CreateSports();
            ClearDataBase(context);
        }

        private void CreateSports()
        {
            sportA = Mock.Of<Sport>(s => s.Id == 1 && s.Name == "SportA");
            sportB = Mock.Of<Sport>(s => s.Id == 2 && s.Name == "SportB");
            Mock.Get(sportA).Setup(s => s.GetTeams()).Returns(mockTeamsA.GetEnumerator());
            Mock.Get(sportB).Setup(s => s.GetTeams()).Returns(mockTeamsB.GetEnumerator());
        }

        private void CreateTeams()
        {
            team1 = Mock.Of<Team>(t => t.Id == 1 && t.Name == "TeamA" && t.Photo == "SomePhoto");
            team2 = Mock.Of<Team>(t => t.Id == 2 && t.Name == "TeamB" && t.Photo == "SomePhoto");
            team3 = Mock.Of<Team>(t => t.Id == 3 && t.Name == "TeamC" && t.Photo == "SomePhoto");
            team4 = Mock.Of<Team>(t => t.Id == 4 && t.Name == "TeamD" && t.Photo == "SomePhoto");
            Mock.Get(team1).Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 1);
            Mock.Get(team2).Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 2);
            Mock.Get(team3).Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 3);
            Mock.Get(team4).Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 4);
            mockTeamsA = new List<Team> { team1, team2 };
            mockTeamsB = new List<Team> { team3, team4 };
        }

        private DatabaseConnection CreateContext()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                            .UseInMemoryDatabase(databaseName: "SportRepository")
                            .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            sportStorage = new SportRepository(context);
            return context;
        }

        private void ClearDataBase(DatabaseConnection context)
        {
            foreach (SportEntity sport in context.Sports)
            {
                context.Sports.Remove(sport);
            }
            context.SaveChanges();
        }

        [TestMethod]
        public void NoSportsTest()
        {
            bool noSports = sportStorage.IsEmpty();
            Assert.IsTrue(noSports);
        }

        [TestMethod]
        public void AddSportTest()
        {
            sportStorage.Add(sportA);
            Assert.AreEqual(1, sportStorage.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(SportAlreadyExistsException))]
        public void AddAlreadyExistentTeamTest()
        {
            sportStorage.Add(sportA);
            sportStorage.Add(sportA);
        }

        [TestMethod]
        public void GetSportTest()
        {
            ISportRepository specific = (ISportRepository)sportStorage;
            sportStorage.Add(sportA);
            Sport sportInDb = specific.GetTeamByName("SportA");
            Assert.AreEqual(sportA, sportInDb);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void GetNotExistentTeamTest()
        {
            ISportRepository specific = (ISportRepository)sportStorage;
            Sport teamInDb = specific.GetTeamByName("DreamTeam");
        }

        [TestMethod]
        public void ExistsTeamTest()
        {
            sportStorage.Add(sportA);
            bool result = sportStorage.Exists(sportA);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesNotExistTest()
        {

            sportStorage.Add(sportA);
            bool result = sportStorage.Exists(sportB);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteTest()
        {
            sportStorage.Add(sportA);
            sportStorage.Delete(sportA.Id);
            Assert.IsTrue(sportStorage.IsEmpty());
        }


        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void DeleteNotExistentTest()
        { 
            sportStorage.Add(sportA);
            sportStorage.Delete(sportA.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void DeleteByIdNotExistentTest()
        {
            sportStorage.Add(sportA);
            sportStorage.Delete(sportB.Id);
        }

        [TestMethod]
        public void ModifySportTest()
        {
            sportStorage.Add(sportA);
            sportA.Name = "SportAcus";
            sportStorage.Modify(sportA);
            Sport editedSport = sportStorage.Get(sportA.Id);
            Assert.AreEqual(sportA.Name, editedSport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void ModifyNotExistentTest()
        {
            sportStorage.Modify(sportA);
        }

        [TestMethod]
        public void ClearTest()
        {

            sportStorage.Add(sportA);
            sportStorage.Add(sportB);

            sportStorage.Clear();
            Assert.IsTrue(sportStorage.IsEmpty());
        }

        [TestMethod]
        public void GetAllTest()
        {
      
            sportStorage.Add(sportA);
            sportStorage.Add(sportB);

            ICollection<Sport> sports = sportStorage.GetAll();

            Assert.AreEqual(2, sports.Count);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            sportStorage.Add(sportA);
            sportStorage = (ISportRepository)sportStorage;
            Sport sportInDb = sportStorage.Get(sportA.Id);
            Assert.AreEqual("SportA", sportA.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void GetByIdNotExistentSportTest()
        {
            Sport sportsInDb = sportStorage.Get(sportA.Id);
        }
    }
}
