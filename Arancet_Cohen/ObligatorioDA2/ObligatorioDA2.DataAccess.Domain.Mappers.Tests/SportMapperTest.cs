using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers.Tests
{
    [TestClass]
    public class SportMapperTest
    {
        private SportMapper testMapper;
        private Mock<Sport> sport;
        private SportEntity entity;
        [TestInitialize]
        public void SetUp() {
            testMapper = new SportMapper();
            sport = new Mock<Sport>("Soccer");
        }

    }
}
