using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObligatorioDA2.BusinessLogic.Test
{
    [TestClass]
    public class EncounterFactoryTest
    {
        private MatchFactory testFactory;

        [TestInitialize]
        public void SetUp() {
            testFactory = new MatchFactory();
        }


    }
}
