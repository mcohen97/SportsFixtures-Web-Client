using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using RepositoryInterface;
using DataRepositories;

namespace DataRepositoriesTest
{
    [TestClass]
    public class CommentRepositoryTest
    {
        ICommentRepository comments;
        
        [TestInitialize]
        public void SetUp() {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "CommentRepository")
                .Options;
            DatabaseConnection db = new DatabaseConnection(options);
            IEntityRepository<CommentEntity> entityRepo = new GenericRepository<CommentEntity>();
        }

    }
}
