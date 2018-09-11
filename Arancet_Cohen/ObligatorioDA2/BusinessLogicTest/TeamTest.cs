using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;

namespace BusinessLogicTest
{
    [TestClass]
    public class TeamTest
    {
        [TestMethod]
        public void ConstructorTeamTest()
        {
            Team team = new Team();
            Assert.IsNotNull(team);
        }

        [TestMethod]
        public void ConstructorParametersTeamTest()
        {
            Team team = new Team();
            Assert.IsNotNull(team);
        }
    }
}
