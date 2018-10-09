using ObligatorioDA2.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.DataAccess.Entities;
using System.Collections.Generic;

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
            entity = new SportEntity()
            {
                Name = "Soccer",
            };
        }
    

        [TestMethod]
        public void SportToEntityNameTest() {
            SportEntity converted = testMapper.ToEntity(sport.Object);
            Assert.AreEqual(converted.Name, entity.Name);
         }
     

        [TestMethod]
        public void EntityToSportNameTest() {
            Sport converted = testMapper.ToSport(entity);
            Assert.AreEqual(converted.Name, sport.Object.Name);
        }

    }
}
