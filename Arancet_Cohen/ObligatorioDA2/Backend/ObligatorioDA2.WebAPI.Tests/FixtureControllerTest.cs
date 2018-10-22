using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.Services.Interfaces;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class FixtureControllerTest
    {
        private FixtureController controller;
        private Mock<IFixtureService> fixtureService;
        private Mock<IOptions<FixtureStrategies>> settings;

        [TestInitialize]
        public void SetUp() {
            FixtureControllerTest = new FixtureController(fixtureService, settings);
        }
    }
}
