using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.BusinessLogic;
using System.Collections.Generic;
using Moq;
using ObligatorioDA2.BusinessLogic.Exceptions;

namespace BusinessLogicTest
{
    [TestClass]
    public class SportTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            Sport sport = new Sport("Soccer");
            Assert.IsNotNull(sport);
        }

        [TestMethod]
        public void ConstructorWithParametersTest()
        {
            string name = "Football";
            Sport sport = new Sport(name);
            Assert.IsNotNull(sport);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Sport aSport = new Sport("SportA");
            Sport sameSport = new Sport("SportA");
            Assert.AreEqual(aSport, sameSport);
        }

        [TestMethod]
        public void NotEqualsTest()
        {
            Sport aSport = new Sport("SportA");
            Sport differentSport = new Sport("SportB");
            Assert.AreNotEqual(aSport, differentSport);
        }

        [TestMethod]
        public void NotEqualsNullTest()
        {
            Sport aSport = new Sport("SportA");
            Assert.IsFalse(aSport.Equals(null));
        }

        [TestMethod]
        public void NotEqualsDifferentTypeTest()
        {
            Sport aSport = new Sport("SportA");
            Assert.IsFalse(aSport.Equals("different type"));
        }

        [TestMethod]
        public void SetNameTest()
        {
            Sport aSport = new Sport("SportA");
            string newName = "NewName";
            aSport.Name = newName;
            Assert.AreEqual(newName, aSport.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void SetEmptyNameTest()
        {
            Sport sport = new Sport("Name");
            sport.Name = "";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSportDataException))]
        public void SetNullNameTest()
        {
            Sport sport = new Sport("Name");
            sport.Name = null;
        }


    }
}
