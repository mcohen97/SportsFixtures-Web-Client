using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using DataAccess;
using DataRepositories;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.WebAPI.Controllers;
using ObligatorioDA2.WebAPI.Models;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class SportsControllerTest
    {
        private Mock<ISportRepository> repo;
        private SportsController controllerToTest;

        [TestInitialize]
        public void SetUp() {
            SetUpRepository();
        }

        private void SetUpRepository()
        {
            repo = new Mock<ISportRepository>();

            Sport testSport1 = new Sport(2,"Tennis");
            Sport testSport2 = new Sport(3, "Basketball");

            repo.Setup(r => r.Get(2)).Returns(testSport1);
            repo.Setup(r => r.Get(It.Is<int>(x => (x != 2) && (x !=3)))).Throws(new SportNotFoundException());
            repo.Setup(r => r.GetAll()).Returns(new List<Sport>() {new Sport(3,"Basketball"), new Sport(2,"Tennis") });

            controllerToTest = new SportsController(repo.Object);
        }

        [TestMethod]
        public void CreateSportTest() {
            SportModelIn input= new SportModelIn()
            {
                Name = "Soccer"
            };

            IActionResult result = controllerToTest.Post(input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            SportModelOut output = createdResult.Value as SportModelOut;

            repo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(createdResult.StatusCode, 201);
            Assert.AreEqual(createdResult.RouteName, "GetById");
            Assert.IsNotNull(output);
            Assert.AreEqual(output.Name, input.Name);
        }

        [TestMethod]
        public void CreateInvalidSportTest() {
            SportModelIn input = new SportModelIn();

            controllerToTest.ModelState.AddModelError("", "Error");
            IActionResult result = controllerToTest.Post(input);

            BadRequestObjectResult createdResult = result as BadRequestObjectResult;

            repo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Never);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void GetSportTest() {

            IActionResult result = controllerToTest.Get(2);
            OkObjectResult okResult = result as OkObjectResult;
            SportModelOut modelOut = okResult.Value as SportModelOut;

            repo.Verify(r => r.Get(2), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(modelOut.Name, "Tennis");
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void GetNotExistentTest() {
            IActionResult result = controllerToTest.Get(5);
            NotFoundObjectResult notFound = result as NotFoundObjectResult;

            repo.Verify(r => r.Get(5), Times.Once);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(notFound.StatusCode, 404);
        }

        [TestMethod]
        public void GetAlltest() {
            IActionResult result = controllerToTest.Get();

            OkObjectResult okResult = result as OkObjectResult;
            IEnumerable<SportModelOut> resultObject = okResult.Value as IEnumerable<SportModelOut>;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(resultObject);
        }

        [TestMethod]
        public void DeleteTest() {
            IActionResult result = controllerToTest.Delete(2);

            OkResult okResult = result as OkResult;

            repo.Verify(r => r.Delete(2), Times.Once);
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public void DeleteNotFoundTest() {
            repo.Setup(r => r.Delete(5)).Throws(new SportNotFoundException());

            IActionResult result = controllerToTest.Delete(5);
            NotFoundObjectResult notFound = result as NotFoundObjectResult;

            repo.Verify(r => r.Delete(5), Times.Once);
            Assert.IsNotNull(notFound);
            Assert.IsNotNull(notFound.Value);
        }
    }
}
