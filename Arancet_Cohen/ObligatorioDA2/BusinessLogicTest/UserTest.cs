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
            toTest = new Mock<User>("name", "surname", "username", "password", "mail@domain.com");
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

            toTest = new Mock<User>("", "surname", "username", "password", "mail@domain.com");
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
            toTest = new Mock<User>("name", "", "username", "password", "mail@domain.com");
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
            toTest = new Mock<User>("name", "surname", "", "password", "mail@domain.com");
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
            toTest = new Mock<User>("name", "surname", "username", "", "mail@domain.com");
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
        public void GetEmailTest()
        {
            Assert.AreEqual("mail@domain.com", toTest.Object.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetEmptyEmailTest()
        {
            toTest = new Mock<User>("name", "surname", "username", "password", "");
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
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetEmailWithNoDomainTest()
        {
            toTest = new Mock<User>("name", "surname", "username", "password", "mail");
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
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetEmailWithNoNameTest()
        {
            toTest = new Mock<User>("name", "surname", "username", "password", "@domain.com");
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
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetEmailWithNoDomainTagTest()
        {
            toTest = new Mock<User>("name", "surname", "username", "password", "@domain");
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
        [ExpectedException(typeof(InvalidUserDataException))]
        public void SetEmailWithNoAtTest()
        {
            toTest = new Mock<User>("name", "surname", "username", "password", "maildomain.com");
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
        public void IsAdminTest() {
            toTest.Setup(u => u.IsAdmin()).Returns(false);
            Assert.IsFalse(toTest.Object.IsAdmin());
        }

        [TestMethod]
        public void IsNotAdminTest() {
            toTest.Setup(u => u.IsAdmin()).Returns(true);
            Assert.IsTrue(toTest.Object.IsAdmin());
        }

    }
}
