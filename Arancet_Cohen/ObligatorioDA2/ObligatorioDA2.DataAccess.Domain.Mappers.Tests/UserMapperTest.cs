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
        Mock<User> toStore;
        UserEntity toGet;
        UserMapper toTest;

        [TestInitialize]
        public void StartUp() {
            toStore = new Mock<User>("name", "surname", "username", "password", "email@domain.com");
            toGet = new UserEntity()
            {
                Name = "name",
                Surname = "surname",
                UserName = "username",
                Password = "password",
                Email = "email@domain.com"
            };
            toTest = new UserMapper();
        }

        [TestMethod]
        public void UserToEntityNameTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore.Object);
            Assert.AreEqual(conversion.Name, toStore.Object.Name);
        }

        [TestMethod]
        public void UserToEntitySurnameTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore.Object);
            Assert.AreEqual(conversion.Surname, toStore.Object.Surname);
        }

        [TestMethod]
        public void UserToEntityUserNameTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore.Object);
            Assert.AreEqual(conversion.UserName, toStore.Object.UserName);
        }

        [TestMethod]
        public void UserToEntityPasswordTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore.Object);
            Assert.AreEqual(conversion.Password, toStore.Object.Password);
        }

        [TestMethod]
        public void UserToEntityEmailTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore.Object);
            Assert.AreEqual(conversion.Email, toStore.Object.Email);
        }

        [TestMethod]
        public void EntityToUserNameTest() {
            User conversion = toTest.ToUser(toGet);
            Assert.AreEqual(conversion.Name, toGet.Name);
        }

        [TestMethod]
        public void EntityToUserSurnameTest()
        {
            User conversion = toTest.ToUser(toGet);
            Assert.AreEqual(conversion.Surname, toGet.Surname);
        }

        [TestMethod]
        public void EntityToUserUserNameTest()
        {
            User conversion = toTest.ToUser(toGet);
            Assert.AreEqual(conversion.UserName, toGet.UserName);
        }

        [TestMethod]
        public void EntityToUserPasswordTest()
        {
            User conversion = toTest.ToUser(toGet);
            Assert.AreEqual(conversion.Password, toGet.Password);
        }

        [TestMethod]
        public void EntityToUserEmailTest()
        {
            User conversion = toTest.ToUser(toGet);
            Assert.AreEqual(conversion.Email, toGet.Email);
        }
    }
}
