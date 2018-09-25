using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using RepositoryInterface;
using DataRepositories;
using Moq;
using BusinessLogic;
using DataRepositoryInterfaces;

namespace DataRepositoriesTest
{
    [TestClass]
    public class CommentRepositoryTest
    {
        ICommentRepository commentsRepo;
        Commentary comment;
        //Mock<BusinessLogic.Match> match;
        
        [TestInitialize]
        public void SetUp() {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "CommentRepository")
                .Options;
            DatabaseConnection db = new DatabaseConnection(options);
            IEntityRepository<CommentEntity> entityRepo = new GenericRepository<CommentEntity>();
            commentsRepo = new CommentRepository(entityRepo);
            comment = new Commentary(4,"this is a comment");
            commentsRepo.Clear();
            //Mock<Team> home = new Mock<Team>(3,"River Plate", "aPath");
            //Mock<Team> away = new Mock<Team>(3, "Boca Juniors", "aPath");
            //match = new Mock<BusinessLogic.Match>(home.Object, away.Object);
        }

        [TestMethod]
        public void EmptyRepositoryTest() {
            Assert.IsTrue(commentsRepo.IsEmpty());
        }

        [TestMethod]
        public void AddCommentTest() {
            commentsRepo.AddComment(comment);
            Assert.IsFalse(commentsRepo.IsEmpty());
        }

    }
}
