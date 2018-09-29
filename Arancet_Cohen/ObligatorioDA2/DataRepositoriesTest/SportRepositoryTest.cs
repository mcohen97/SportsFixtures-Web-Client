using BusinessLogic;
using DataAccess;
using DataRepositories;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
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
        private ISportRepository sportStorage;
        private Mock<Sport> sportA;
        private Mock<Sport> sportB;
        private ICollection<Team> mockTeamsA;
        private ICollection<Team> mockTeamsB;
        private Mock<Team> team1;
        private Mock<Team> team2;
        private Mock<Team> team3;
        private Mock<Team> team4;

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
            sportA = new Mock<Sport>("SportA");
            sportB = new Mock<Sport>( "SportB");
         }

        private void CreateTeams()
        {
            team1 = new Mock<Team>(1,"TeamA", "SomePhoto");
            team2 = new Mock<Team>(2 , "TeamB" , "SomePhoto");
            team3 = new Mock<Team>(3, "TeamC", "SomePhoto");
            team4 = new Mock<Team>(4, "TeamD", "SomePhoto");
            team1.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 1);
            team2.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 2);
            team3.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 3);
            team4.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == 4);
            mockTeamsA = new List<Team> { team1.Object, team2.Object };
            mockTeamsB = new List<Team> { team3.Object, team4.Object };
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
            sportStorage.Add(sportA.Object);
            Assert.AreEqual(1, sportStorage.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(SportAlreadyExistsException))]
        public void AddAlreadyExistentTeamTest()
        {
            sportStorage.Add(sportA.Object);
            sportStorage.Add(sportA.Object);
        }

        [TestMethod]
        public void GetSportTest()
        {
            ISportRepository specific = (ISportRepository)sportStorage;
            sportStorage.Add(sportA.Object);
            Sport sportInDb = specific.Get("SportA");
            Assert.AreEqual(sportA.Object.Name, sportInDb.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void GetNotExistentTeamTest()
        {
            ISportRepository specific = (ISportRepository)sportStorage;
            Sport teamInDb = specific.Get("DreamSport");
        }

        [TestMethod]
        public void ExistsTeamTest()
        {
            sportStorage.Add(sportA.Object);
            bool result = sportStorage.Exists(sportA.Object);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesNotExistTest()
        {

            sportStorage.Add(sportA.Object);
            bool result = sportStorage.Exists(sportB.Object);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteTest()
        {
            sportStorage.Add(sportA.Object);
            sportStorage.Delete(sportA.Object.Name);
            Assert.IsTrue(sportStorage.IsEmpty());
        }


        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void DeleteNotExistentTest()
        { 
            sportStorage.Add(sportA.Object);
            sportStorage.Delete(sportB.Object.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void DeleteByIdNotExistentTest()
        {
            sportStorage.Add(sportA.Object);
            sportStorage.Delete(sportB.Object.Name);
        }

        [TestMethod]
        public void ModifySportTest()
        {
            sportStorage.Add(sportA.Object);
            sportA.Name = "SportAcus";
            CreateContext();
            sportStorage.Modify(sportA.Object);
            Sport editedSport = sportStorage.Get(sportA.Object.Name);
            Assert.AreEqual(sportA.Object.Name, editedSport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void ModifyNotExistentTest()
        {
            sportStorage.Modify(sportA.Object);
        }

        [TestMethod]
        public void ClearTest()
        {

            sportStorage.Add(sportA.Object);
            sportStorage.Add(sportB.Object);

            sportStorage.Clear();
            Assert.IsTrue(sportStorage.IsEmpty());
        }

        [TestMethod]
        public void GetAllTest()
        {
      
            sportStorage.Add(sportA.Object);
            sportStorage.Add(sportB.Object);

            ICollection<Sport> sports = sportStorage.GetAll();

            Assert.AreEqual(2, sports.Count);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            sportStorage.Add(sportA.Object);
            ISportRepository specific = (ISportRepository) sportStorage;
            Sport sportInDb = specific.Get(sportA.Object.Name);
            Assert.AreEqual("SportA", sportA.Object.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void GetByIdNotExistentSportTest()
        {
            Sport sportsInDb = sportStorage.Get(sportA.Object.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(SportNotFoundException))]
        public void GetByNameNotExistentSportTest()
        {
            ISportRepository specific = (ISportRepository)sportStorage;
            Sport sportsInDb = specific.Get(sportA.Name);
        }
    }
}
