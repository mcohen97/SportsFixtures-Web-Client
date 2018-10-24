using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.Data.Entities;
using ObligatorioDA2.Data.DomainMappers;

namespace ObligatorioDA2.Data.DataAccess.Mappers.Tests
{
    [TestClass]
    public class LogInfoMapperTest
    {
        private LogInfoMapper mapper;
        private LogInfoEntity entity;
        private LogInfo log;

        [TestInitialize]
        public void Initialize()
        {
            mapper = new LogInfoMapper();
            entity = new LogInfoEntity()
            {
                Id = 1,
                Messagge = "User logged using API",
                LogType = LogType.LOGIN,
                Username = "admin",
                Date = DateTime.Now
            }; ;
            log = new LogInfo()
            {
                Id = 1,
                Messagge = "User logged using API",
                LogType = LogType.LOGIN,
                Username = "admin",
                Date = DateTime.Now
            };
        }

        [TestMethod]
        public void LogInfoToEntityTest()
        {
            LogInfoEntity converted = mapper.ToEntity(log);
            Assert.AreEqual(log.Id, converted.Id);
            Assert.AreEqual(log.Messagge, converted.Messagge);
            Assert.AreEqual(log.LogType, converted.LogType);
            Assert.AreEqual(log.Username, converted.Username);
            Assert.AreEqual(log.Date, converted.Date);
        }

        [TestMethod]
        public void EntityToLogInfoTest()
        {
            LogInfo converted = mapper.ToLogInfo(entity);
            Assert.AreEqual(entity.Id, converted.Id);
            Assert.AreEqual(entity.Messagge, converted.Messagge);
            Assert.AreEqual(entity.LogType, converted.LogType);
            Assert.AreEqual(entity.Username, converted.Username);
            Assert.AreEqual(entity.Date, converted.Date);
        }
    }
}
