using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.DataAccess;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace DataRepositoriesTest
{
    [TestClass]
    public class SportRepositoryTest
    {
        private ISportRepository sportStorage;
        private Mock<Sport> sportA;
        private Mock<Sport> sportB;
 

        [TestInitialize]
        public void TestInitialize()
        {
            DatabaseConnection context = CreateContext();
            CreateSports();
            ClearDataBase(context);
        }

        private void CreateSports()
        {
            sportA = new Mock<Sport>("SportA",true);
            sportB = new Mock<Sport>("SportB",false);
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

        private void CreateDisconnectedDatabase()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "UserRepositoryTest")
                .Options;
            Mock<DatabaseConnection> contextMock = new Mock<DatabaseConnection>(options);
            Mock<DbException> toThrow = new Mock<DbException>();
            contextMock.Setup(c => c.Sports).Throws(toThrow.Object);
            sportStorage = new SportRepository(contextMock.Object);
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
        [ExpectedException(typeof(DataInaccessibleException))]
        public void IsEmptyNoAccessTest()
        {
            CreateDisconnectedDatabase();
            sportStorage.IsEmpty();
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
        [ExpectedException(typeof(DataInaccessibleException))]
        public void AddSportNoAccessTest()
        {
            CreateDisconnectedDatabase();
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
            Sport teamInDb = sportStorage.Get("DreamSport");
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void GetSportNoAccessTest()
        {
            CreateDisconnectedDatabase();
            sportStorage.Get("DreamSport");
        }

        [TestMethod]
        public void ExistsTeamTest()
        {
            sportStorage.Add(sportA.Object);
            bool result = sportStorage.Exists(sportA.Object.Name);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesNotExistTest()
        {

            sportStorage.Add(sportA.Object);
            bool result = sportStorage.Exists(sportB.Object.Name);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DataInaccessibleException))]
        public void ExistsNoAccessTest()
        {
            CreateDisconnectedDatabase();
            sportStorage.Exists("DreamSport");
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
        [ExpectedException(typeof(DataInaccessibleException))]
        public void DeleteNoAccessTest()
        {
            CreateDisconnectedDatabase();
            sportStorage.Delete("DreamSport");
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
        [ExpectedException(typeof(DataInaccessibleException))]
        public void ModifyNoAccessTest()
        {
            CreateDisconnectedDatabase();
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
        [ExpectedException(typeof(DataInaccessibleException))]
        public void ClearNoAccessTest()
        {
            CreateDisconnectedDatabase();
            sportStorage.Clear();
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
        [ExpectedException(typeof(DataInaccessibleException))]
        public void GetAllNoAccessTest()
        {
            CreateDisconnectedDatabase();
            sportStorage.GetAll();
        }
    }
}
