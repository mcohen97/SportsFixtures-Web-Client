using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccessTest
{
    public class DbContextExample : DbContext { }

    [TestClass]
    public class ContextFactoryTest
    {
        ContextFactory factory;

        [TestInitialize]
        public void SetUp() {
            factory = new ContextFactory();  
        }

        [TestMethod]
        public void GetDefaultContextTest(){
            factory = new ContextFactory();
            DatabaseConnection db = factory.Get();
            Assert.IsNotNull(db);
            db.Dispose();
        }

        [TestMethod]
        public void SetInMemoryTest() {
            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
            factory = new ContextFactory(options);
            DatabaseConnection db = factory.Get();
            Assert.IsNotNull(db);
            db.Dispose();
        }
    }
}
