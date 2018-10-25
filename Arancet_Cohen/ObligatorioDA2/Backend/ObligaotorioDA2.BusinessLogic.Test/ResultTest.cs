using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObligatorioDA2.BusinessLogic.Test
{
    [TestClass]
    public class ResultTest
    {
        private Result testResult;
        private Team testTeam;

        [TestInitialize]
        public void StartUp() {
            testResult = new Result();
            Sport sportDummy = new Sport("aSport", true);
            testTeam = new Team("aTeam", "aPhoto", sportDummy);
        }

        [TestMethod]
        public void AddPositionTest() {
            testResult.Add(testTeam,1);
            Assert.IsTrue(testResult.GetPositions().Any());
        }
    }
}
