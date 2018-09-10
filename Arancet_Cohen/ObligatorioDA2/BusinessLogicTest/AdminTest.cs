using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using Moq;

namespace BusinessLogicTest
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void IsAdminTest()
        {
            Mock<User> fake = new Mock<User>("name", "surname", "username", "password", "mail@domain.com");
            Assert.IsTrue(fake.Object.IsAdmin);
        }
    }
}
