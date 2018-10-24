using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}
