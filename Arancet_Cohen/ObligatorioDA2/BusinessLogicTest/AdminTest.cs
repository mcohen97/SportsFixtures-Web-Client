﻿using System;
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
            Admin test= new Admin("name", "surname", "username", "password", "mail@domain.com");
            Assert.IsTrue(test.IsAdmin());
        }
    }
}