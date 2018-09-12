using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccessTest
{
    [TestClass]
    public class ContextFactoryTest
    {
        class DbContextExcample:DbContext { }

        ContextFactory factory;

        [TestInitialize]
        public void SetUp() {
            factory = new ContextFactory();
            
        }

        [TestMethod]
        public void RegisterTest()
        {
            Mock<DbContextExcample> fake = new Mock<DbContextExcample>();
            factory.Register<DbContextExcample>(fake.Object);
            DbContext db = factory.Get<DbContextExcample>();
            Assert.Equals(db.GetType(), typeof(DbContextExcample));
        }
    }
}
