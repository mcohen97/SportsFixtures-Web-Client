using System;
using BusinessLogicTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using BusinessLogicExceptions;

namespace BusinessLogicTest
{
    [TestClass]
    public class UserTest
    {
        User toTest;

        [TestInitialize]
        public void SetUp()
        {
            toTest = new Admin("name", "surname", "username", "password", "email");
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
            toTest = new Admin("", "surname", "username", "password", "email");
        }

    }
}
