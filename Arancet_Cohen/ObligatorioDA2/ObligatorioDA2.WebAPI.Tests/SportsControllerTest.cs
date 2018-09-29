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

            Sport testSport1 = new Sport("Tennis");
            Sport testSport2 = new Sport("Basketball");

            repo.Setup(r => r.Get("Tennis")).Returns(testSport1);
            repo.Setup(r => r.Get(It.Is<String>(x => (x != "Tennis") && (x !="Basketball")))).Throws(new SportNotFoundException());
            repo.Setup(r => r.GetAll()).Returns(new List<Sport>() {new Sport("Basketball"), new Sport("Tennis") });

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

            IActionResult result = controllerToTest.Get("Tennis");
            OkObjectResult okResult = result as OkObjectResult;
            SportModelOut modelOut = okResult.Value as SportModelOut;

            repo.Verify(r => r.Get("Tennis"), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(modelOut.Name, "Tennis");
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void GetNotExistentTest() {
            IActionResult result = controllerToTest.Get("Golf");
            NotFoundObjectResult notFound = result as NotFoundObjectResult;

            repo.Verify(r => r.Get("Golf"), Times.Once);
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
            IActionResult result = controllerToTest.Delete("Tennis");

            OkResult okResult = result as OkResult;

            repo.Verify(r => r.Delete("Tennis"), Times.Once);
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public void DeleteNotFoundTest() {
            repo.Setup(r => r.Delete("Golf")).Throws(new SportNotFoundException());

            IActionResult result = controllerToTest.Delete("Golf");
            NotFoundObjectResult notFound = result as NotFoundObjectResult;

            repo.Verify(r => r.Delete("Golf"), Times.Once);
            Assert.IsNotNull(notFound);
            Assert.IsNotNull(notFound.Value);
        }

        [TestMethod]
        public void PutModifyTest() {

            SportModelIn input = new SportModelIn() { Name = "Soccer" };
            IActionResult result = controllerToTest.Put("Soccer",input);

            OkObjectResult okResult = result as OkObjectResult;

            repo.Verify(r => r.Modify(It.IsAny<Sport>()), Times.Once);
            repo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Never);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
        }

        [TestMethod]
        public void PutAddTest() {
            repo.Setup(r => r.Modify(It.IsAny<Sport>())).Throws(new SportNotFoundException());
            SportModelIn input = new SportModelIn() { Name = "Soccer" };
            IActionResult result = controllerToTest.Put("Soccer",input);

            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;

            repo.Verify(r => r.Modify(It.IsAny<Sport>()), Times.Once);
            repo.Verify(r => r.Add(It.IsAny<Sport>()), Times.Once);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(createdResult.StatusCode, 201);
            Assert.AreEqual(createdResult.RouteName, "GetById");
        }

        [TestMethod]
        public void PutInvalidTest() {
            SportModelIn input = new SportModelIn() {};
            controllerToTest.ModelState.AddModelError("", "Error");
            IActionResult result = controllerToTest.Put("Soccer",input);


            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }
    }
}
