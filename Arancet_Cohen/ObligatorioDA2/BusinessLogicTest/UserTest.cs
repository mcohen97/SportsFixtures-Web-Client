using System;
using BusinessLogicTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using BusinessLogicExceptions;
using Moq;
using System.Reflection;

namespace BusinessLogicTest
{
    class ConcreteUser : User
    {
        public ConcreteUser(string aName, string aSurname, string aUserName, string aPassword, string aEmail) : base(aName, aSurname, aUserName, aPassword, aEmail) {
        }
    }

    [TestClass]
    public class UserTest
    {
        Mock<User> toTest;

        [TestInitialize]
        public void SetUp()
        {
            toTest = new Mock<User>("name", "surname", "username", "password", "email");
            toTest.CallBase=true;
  
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
            
            toTest.CallBase = true;
            toTest= new Mock<User>("", "surname", "username", "password", "email");
            try
            {
                var o = toTest.Object;
            }
            catch (TargetInvocationException e) {
                throw e.InnerException;
            }
        }

    }
}
