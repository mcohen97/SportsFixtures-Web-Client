using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers.Tests
{
    [TestClass]
    public class CommentMapperTest
    {
        private CommentMapper testMapper;
        private Commentary comment;
        private CommentEntity commentEntity;
        [TestInitialize]
        public void StartUp() {
            testMapper = new CommentMapper();
            Mock<User> stub = new Mock<User>("aName", "aSurname", "aUsername", "aPassword", "anEmail@aDomain.com");
            comment = new Commentary("this is a comment", stub.Object);
        }

        [TestMethod]
        public void CommentToEntityTextTest() {
            CommentEntity entity = testMapper.ToEntity(comment);
            Assert.AreEqual(entity.Text, comment.Text);
        }

        [TestMethod]
        public void CommentToEntityUserTest()
        {
            CommentEntity entity = testMapper.ToEntity(comment);
            Assert.AreEqual(entity.Maker.UserName, comment.Maker);
        }

        [TestMethod]
        public void CommentToEntityIdTest() {
            CommentEntity entity = testMapper.ToEntity(comment);
            Assert.AreEqual(entity.Id, comment.Id);
        }
    }
}
