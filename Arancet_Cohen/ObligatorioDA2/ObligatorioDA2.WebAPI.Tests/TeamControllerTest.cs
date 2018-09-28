using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RepositoryInterface;
using BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class TeamControllerTest
    {
        private TeamController controller;
        private Mock<IRepository<Team>> repo;
        Team team;

        [TestInitialize]
        public void SetUp() {
            team = new Team(2,"Nacional", "aPath");
            repo = new Mock<IRepository<Team>>();
            repo.Setup(r => r.Get(2)).Returns(team);
            repo.Setup(r => r.Get(It.Is<int>(i=>i != 2))).Throws(new TeamNotFoundException());
            controller = new TeamController(repo.Object);
        }

        [TestMethod]
        public void CreateValidTeamTest()
        {
            //Arrange
            var modelIn = new TeamModelIn() {
                 Name = "DreamTeam", 
                 Photo = "/MyResource/DreamTeam.png"
            };
            var result = controller.Post(modelIn);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as TeamModelOut;

            //Assert
            repo.Verify(r => r.Add(It.IsAny<Team>()), Times.Once);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetById", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(modelIn.Name, modelOut.Name);
        }

        [TestMethod]
        public void CreateFailedTeamRequiredNameTest()
        {
            //Arrange
           var modelIn = new TeamModelIn() {
                 Photo = "/MyResource/DreamTeam.png"
            };
            //We need to force the error in de ModelState
            controller.ModelState.AddModelError("", "Error");
            var result = controller.Post(modelIn);
            //Act
            var createdResult = result as BadRequestObjectResult;
            //Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void GetTeamOk() {

            IActionResult result = controller.Get(2) as IActionResult;
            OkObjectResult okResult = result as OkObjectResult;
            TeamModelOut resultTeam = okResult.Value as TeamModelOut;

            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(201,okResult.StatusCode);
            Assert.AreEqual(resultTeam.Name, team.Name);
        }

        [TestMethod]
        public void GetTeamNotFoundTest() {
            IActionResult result = controller.Get(15) as IActionResult;
            NotFoundObjectResult notFoundResult = result as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, 404);
        }
    }
}