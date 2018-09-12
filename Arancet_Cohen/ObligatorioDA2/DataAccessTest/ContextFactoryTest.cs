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
        public void RegisterTest()
        {
            Mock<DbContextExample> fake = new Mock<DbContextExample>();
            factory.Register<DbContextExample>(fake.Object);
            DbContext db = factory.Get<DbContextExample>();
            Assert.AreEqual(db.GetType(), fake.Object.GetType());
        }
    }
}
