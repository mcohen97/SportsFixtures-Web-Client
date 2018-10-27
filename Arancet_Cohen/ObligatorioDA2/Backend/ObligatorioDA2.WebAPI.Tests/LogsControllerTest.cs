using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class LogsControllerTest
    {
        private ILoggerService logger;
        private LogsController controller;
        private ICollection<LogInfo> someLogList;

        [TestInitialize]
        public void Initialize()
        {
            logger = Mock.Of<ILoggerService>();
            DateTime someDate = new DateTime(2020, 02, 20);
            string someUsername = "SomePepitoFulanito";
            someLogList = new List<LogInfo>()
            {
                new LogInfo()
                {
                    Id = 1,
                    Date = someDate,
                    LogType = LogType.LOGIN,
                    Message = LogMessage.LOGIN_OK,
                    Username = someUsername
                },
                new LogInfo()
                {
                    Id = 2,
                    Date = someDate,
                    LogType = LogType.LOGIN,
                    Message = LogMessage.LOGIN_USER_NOT_FOUND,
                    Username = someUsername+"Roberto"
                },
                new LogInfo()
                {
                    Id = 3,
                    Date = someDate,
                    LogType = LogType.LOGIN,
                    Message = LogMessage.LOGIN_OK,
                    Username = someUsername
                }
            };
        }

        [TestMethod]
        public void GetLogsTest()
        {
            //Arrange.
            Mock.Get(logger).Setup(l => l.GetAllLogs()).Returns(someLogList);

            //Act.
            IActionResult result = controller.GetAll();
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<LogModelOut> logs = okResult.Value as ICollection<LogModelOut>;

            //Assert.
            Mock.Get(logger).Verify(s => s.GetAllLogs(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(logs);
            Assert.AreEqual(logs.Count, 1);
        }

        [TestMethod]
        public void GetLogsNoDataAccessTest()
        {
            //Arrange.
            Exception toThrow = new DataInaccessibleException();
            Mock.Get(logger).Setup(l => l.GetAllLogs()).Throws(toThrow);

            //Act.
            IActionResult result = controller.GetAll();
            ObjectResult noData = result as ObjectResult;
            ErrorModelOut error = noData.Value as ErrorModelOut;

            //Assert.
            Mock.Get(logger).Verify(s => s.GetAllLogs(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(noData);
            Assert.AreEqual(500, noData.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }
    }
}
