using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;
using ObligatorioDA2.WebAPI.Controllers;
using ObligatorioDA2.WebAPI.Models;
using System;
using System.Collections.Generic;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class LogsControllerTest
    {
        private ILoggerService logger;
        private LogsController controller;
        private ICollection<LogInfoDto> someLogList;

        [TestInitialize]
        public void Initialize()
        {
            logger = Mock.Of<ILoggerService>();
            controller = new LogsController(logger);
            DateTime someDate = new DateTime(2020, 02, 20);
            string someUsername = "SomePepitoFulanito";
            someLogList = new List<LogInfoDto>()
            {
                new LogInfoDto()
                {
                    id = 1,
                    date = someDate,
                    logType = LogType.LOGIN,
                    message = LogMessage.LOGIN_OK,
                    username = someUsername
                },
                new LogInfoDto()
                {
                    id = 2,
                    date = someDate,
                    logType = LogType.LOGIN,
                    message = LogMessage.LOGIN_USER_NOT_FOUND,
                    username = someUsername+"Roberto"
                },
                new LogInfoDto()
                {
                    id = 3,
                    date = someDate,
                    logType = LogType.LOGIN,
                    message = LogMessage.LOGIN_OK,
                    username = someUsername
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
            IEnumerable<LogModelOut> logs = okResult.Value as IEnumerable<LogModelOut>;

            //Assert.
            Mock.Get(logger).Verify(s => s.GetAllLogs(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(logs);
        }

        [TestMethod]
        public void GetLogsNoDataAccessTest()
        {
            //Arrange.
            Exception internalEx = new DataInaccessibleException();
            Exception toThrow = new ServiceException(internalEx.Message, ErrorType.DATA_INACCESSIBLE);
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
