using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;

namespace BusinessLogicTest
{
    [TestClass]
    public class FollowerTest
    {
        [TestMethod]
        public void IsAdmintest()
        {
            User test = new Follower("name", "surname", "username", "password", "mail@domain.com");
            Assert.IsFalse(test.IsAdmin);
        }

    }
}
