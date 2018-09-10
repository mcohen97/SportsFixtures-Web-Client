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
  
        }
        [TestMethod]
        public void GetNameTest()
        {
            Assert.AreEqual("name", toTest.Object.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetNameTest()
        {
            toTest = new Mock<User>("", "surname", "username", "password", "email");
        }

    }
}
