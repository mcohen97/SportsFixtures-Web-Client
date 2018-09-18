using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BusinessLogic;
using BusinessLogic.Factories;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.DataAccess.Domain.Mappers;

namespace ObligatorioDA2.DataAccess.Domain.Mappers.Tests
{
    [TestClass]
    public class UserMapperTest
    {
        User toStore;
        UserEntity toGet;
        UserMapper toTest;
        UserFactory factory;

        [TestInitialize]
        public void StartUp() {
            factory = new UserFactory();
            UserId identity = new UserId
            {
                Name = "name",
                Surname = "surname",
                UserName = "username",
                Password = "password",
                Email = "email@domain.com"
            };
            toStore = factory.CreateAdmin(identity);
            toGet = new UserEntity()
            {
                Name = "name",
                Surname = "surname",
                UserName = "username",
                Password = "password",
                Email = "email@domain.com",
                Id = 1
            };
            toTest = new UserMapper();
        }

        [TestMethod]
        public void UserToEntityNameTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore);
            Assert.AreEqual(conversion.Name, toStore.Name);
        }

        [TestMethod]
        public void UserToEntitySurnameTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore);
            Assert.AreEqual(conversion.Surname, toStore.Surname);
        }

        [TestMethod]
        public void UserToEntityUserNameTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore);
            Assert.AreEqual(conversion.UserName, toStore.UserName);
        }

        [TestMethod]
        public void UserToEntityPasswordTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore);
            Assert.AreEqual(conversion.Password, toStore.Password);
        }

        [TestMethod]
        public void UserToEntityEmailTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore);
            Assert.AreEqual(conversion.Email, toStore.Email);
        }

        [TestMethod]
        public void UserToEntityIdTest()
        {
            UserEntity conversion = toTest.ToEntity(toStore);
            Assert.AreEqual(conversion.Id, toStore.Id);
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

        [TestMethod]
        public void EntityToUserIdTest() {
            User conversion = toTest.ToUser(toGet);
            Assert.AreEqual(conversion.Id, toGet.Id);
        }
    }
}
