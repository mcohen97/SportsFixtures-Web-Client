using System;
using DataRepositoryInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Match = BusinessLogic.Match;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class MatchServiceTest
    {
        private MatchService serviceToTest;
        Mock<IMatchRepository> repoDouble;
        [TestInitialize]
        public void SetUp() {
            repoDouble = new Mock<IMatchRepository>();
            Mock<Match> fakeMatch = BuildMatch();
            serviceToTest = new MatchService(repoDouble.Object);
            repoDouble.Setup(r => r.Get(2)).Returns(fakeMatch.Object);
            //repoDouble.Setup(r => r.Get(It.Is<int>(i => i != 2))).Throws(MatchNotFoundException)
        }

        private Mock<Match> BuildMatch()
        {
            throw new NotImplementedException();
        }
    }
}
