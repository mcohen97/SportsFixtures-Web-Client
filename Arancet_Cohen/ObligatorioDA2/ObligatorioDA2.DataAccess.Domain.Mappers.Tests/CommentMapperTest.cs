using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ObligatorioDA2.DataAccess.Domain.Mappers.Tests
{
    [TestClass]
    public class CommentMapperTest
    {
        private CommentMapper testMapper;
        private Comment comment;
        private CommentEntity commentEntity;
        [TestInitialize]
        public void StartUp() {
            testMapper = new CommentMapper();
            Mock<User> stub = new Mock<User>("aName", "aSurname", "aUsername", "aPassword", "anEmail@aDomain.com");
            comment = new Comment("this is a comment", stub.Object);
        }
    }
}
