using DataAccess;
using DataRepositories;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DataRepositoriesTest
{
    [TestClass]
    public class MatchRepositoryTest
    {
        IMatchRepository matchesStorage;
        Mock<BusinessLogic.Match> match;

        [TestInitialize]
        public void SetUp() {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "TeamRepository")
                .Options;
            DatabaseConnection context = new DatabaseConnection(options);
            matchesStorage = new MatchRepository(context);
            match = new Mock<BusinessLogic.Match>();
            matchesStorage.Clear();
        }

        [TestMethod]
        public void EmptyTest() {
            Assert.IsTrue(matchesStorage.IsEmpty());
        }

        [TestMethod]
        public void AddMatchNotemptyTest() {
            matchesStorage.Add(match.Object);
            Assert.IsFalse(matchesStorage.IsEmpty());
        }

       /* [TestMethod]
        public void AddMatchNotemptyTest()
        {
            matchesStorage.Add(match);
            Assert.IsFalse(matchesStorage.IsEmpty());
        }*/

    }
}
