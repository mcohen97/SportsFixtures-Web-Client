using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.DataAccess.Domain.Mappers;

namespace ObligatorioDA2.DataAccess.Domain.Mappers.Tests
{
    [TestClass]
    public class UserMapperTest
    {
        Mock<User> toConvert;
        UserMapper toTest;

        [TestInitialize]
        public void StartUp() {
            toConvert = new Mock<User>("name", "surname", "username", "password", "email@domain.com");
            toTest = new UserMapper();
        }
        [TestMethod]
        public void UserToEntityNameTest()
        {
            UserEntity conversion = toTest.ToEntity(toConvert.Object);
            Assert.AreEqual(conversion.Name, toConvert.Object.Name);
        }

        public void UserToEntitySurnameTest()
        {
            UserEntity conversion = toTest.ToEntity(toConvert.Object);
            Assert.AreEqual(conversion.Surname, toConvert.Object.Surname);
        }
        public void UserToEntityUserNameTest()
        {
            UserEntity conversion = toTest.ToEntity(toConvert.Object);
            Assert.AreEqual(conversion.Surname, toConvert.Object.Surname);
        }
    }
}
