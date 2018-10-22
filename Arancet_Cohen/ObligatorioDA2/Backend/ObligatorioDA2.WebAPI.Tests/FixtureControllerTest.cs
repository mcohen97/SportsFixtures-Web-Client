using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.WebAPI.Controllers;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class FixtureControllerTest
    {
        private FixturesController controller;
        private Mock<IFixtureService> fixtureService;
        private Mock<IOptions<FixtureStrategies>> settings;

        [TestInitialize]
        public void SetUp() {
            controller = new FixturesController(fixtureService.Object, settings.Object);
        }
    }
}
