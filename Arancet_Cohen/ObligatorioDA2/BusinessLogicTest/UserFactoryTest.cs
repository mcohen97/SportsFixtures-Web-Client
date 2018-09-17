using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using System.Collections.Generic;

namespace BusinessLogicTest
{
    [TestClass]
    class UserFactoryTest
    {
        User anAdmin;
        User factoryAdmin;
        User aFollower;
        User factoryFollower;
        UserFactoryTest factory;

        [TestInitialize]
        public void SetUp() {
            factory = new UserFactory();
            anAdmin = new User("name", "surname", "username", "password", "email@domain.com",true);
            factoryAdmin = factory.CreateAdmin("name", "surname", "username", "password", "email@domain.com");
            aFollower = new User("name", "surname", "username", "password", "email@domain.com", false);
            factoryFollower= factory.CreateAdmin("name", "surname", "username", "password", "email@domain.com");
        }

        [TestMethod]
        public void CreateAdminNameTest()
        {
            Assert.AreEqual(factoryAdmin.Name, anAdmin.Name);
        }

        [TestMethod]
        public void CreateAdminSurnameTest()
        {
            Assert.AreEqual(factoryAdmin.Surname, anAdmin.Surname);
        }

        [TestMethod]
        public void CreateAdminUserNameTest()
        {
            Assert.AreEqual(factoryAdmin.UserName, anAdmin.UserName);
        }

        [TestMethod]
        public void CreateAdminPasswordTest()
        {
            Assert.AreEqual(factoryAdmin.Password, anAdmin.Password);
        }

        [TestMethod]
        public void CreateAdminEmailTest()
        {
            Assert.AreEqual(factoryAdmin.Email, anAdmin.Email);
        }

        [TestMethod]
        public void IsAdminTest() {
            Assert.IsTrue(factoryAdmin.IsAdmin());
        }

        [TestMethod]
        public void IsNotAdmin() {
            Assert.IsFalse(factoryAdmin.IsAdmin());
        }
    }
}
