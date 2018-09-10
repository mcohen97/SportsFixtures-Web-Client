using System;
using BusinessLogicTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using BusinessLogicExceptions;
using Moq;
using System.Reflection;

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
            toTest.CallBase = true;

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
            try
            {
                var o = toTest.Object;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        [TestMethod]
        public void GetSurnameTest()
        {
            Assert.AreEqual("surname", toTest.Object.Surname);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetInvalidSurnameTest()
        {
            toTest = new Mock<User>("name", "", "username", "password", "email");
            try
            {
                var o = toTest.Object;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        [TestMethod]
        public void GetUserNameTest()
        {
            Assert.AreEqual("username", toTest.Object.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetInvalidUserNameTest()
        {
            toTest = new Mock<User>("name", "surname", "", "password", "email");
            try
            {
                var o = toTest.Object;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        [TestMethod]
        public void GetPasswordTest()
        {
            Assert.AreEqual("password", toTest.Object.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetInvalidPasswordTest()
        {
            toTest = new Mock<User>("name", "surname", "username", "", "email");
            try
            {
                var o = toTest.Object;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }


    }
}
