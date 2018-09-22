using System;
using DataAccess;
using DataRepositories;
using DataRepositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;
using RepositoryInterface;

namespace DataRepositoriesTest
{
    [TestClass]
    class GenericRepositoryTest
    {
        IRepository<BaseEntity> testRepo;
        Mock<BaseEntity> testEntity;
        [TestInitialize]
        public void SetUp()
        {
            InitializeRepository();
            testEntity = new Mock<BaseEntity>();
            testEntity.SetupGet(u => u.Id).Returns(3);
        }

        private void InitializeRepository()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "UserRepository")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            testRepo = new GenericRepository<BaseEntity>(context);
        }

        [TestMethod]
        public void AddTest() {
            testRepo.Add(testEntity.Object);
            Assert.IsFalse(testRepo.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(EntityAlreadyExistsException))]
        public void AddAlreadyexistingtest() {
            testRepo.Add(testEntity.Object);
            testRepo.Add(testEntity.Object);
        }

        [TestMethod]
        public void GetTest() {
            testRepo.Add(testEntity.Object);
            BaseEntity fromRepo =testRepo.Get(3);
            Assert.AreEqual(fromRepo.Id, 3);
        }
    }
}
