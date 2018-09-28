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

            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(createdResult.StatusCode, 201);
            Assert.AreEqual(createdResult.RouteName, "GetById");
            Assert.IsNotNull(output);
            Assert.AreEqual(output.Name, input.Name);
        }
    }
}
