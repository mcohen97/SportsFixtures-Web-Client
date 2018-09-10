using System;
using BusinessLogicTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using BusinessLogicExceptions;
using Moq;

namespace BusinessLogicTest
{
    [TestClass]
    public class UserTest
    {
        Mock<User> toTest;

        [TestInitialize]
        public void SetUp()
        {
            toTest = new Mock<User>("name", "surname", "username", "password", "email");
            toTest.Setup(u => u.Name).Returns("name");
            /*toTest.Setup(u => u.Surname).Returns("surname");
            toTest.Setup(u => u.Username).Returns("username");
            toTest.Setup(u => u.Password).Returns("password");
            toTest.Setup(u => u.Email).Returns("email");*/
        }
        [TestMethod]
        public void GetNameTest()
        {
            Assert.AreEqual("name", toTest.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetNameTest()
        {
            toTest = new Mock<User>("", "surname", "username", "password", "email");
        }

    }
}
