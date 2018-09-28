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

            repo.Verify(r => r.Get(2), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200,okResult.StatusCode);
            Assert.AreEqual(resultTeam.Name, team.Name);
        }

        [TestMethod]
        public void GetTeamNotFoundTest() {
            IActionResult result = controller.Get(15) as IActionResult;
            NotFoundObjectResult notFoundResult = result as NotFoundObjectResult;

            repo.Verify(r => r.Get(15), Times.Once);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, 404);
        }

        [TestMethod]
        public void PutTest() {
            //Arrange
            var modelIn = new TeamModelIn()
            {
                Name = "DreamTeam",
                Photo = "/MyResource/DreamTeam.png"
            };

            IActionResult result = controller.Put(2,modelIn);
            OkResult okResult = result as OkResult;

            //verify it modifies but not adds
            repo.Verify(r => r.Modify(It.IsAny<Team>()), Times.Once);
            repo.Verify(r => r.Add(It.IsAny<Team>()), Times.Never);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void PutAddTest() {

            //make repository throw not existing exception, so that it has to add
            repo.Setup(r => r.Modify(It.IsAny<Team>())).Throws(new TeamNotFoundException());

            var modelIn = new TeamModelIn()
            {
                Name = "DreamTeam",
                Photo = "/MyResource/DreamTeam.png"
            };

            //act
            IActionResult result =controller.Put(1,modelIn);
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as TeamModelOut;

            //assert
            repo.Verify(r => r.Modify(It.IsAny<Team>()), Times.Once);
            repo.Verify(r => r.Add(It.IsAny<Team>()), Times.Once);
            Assert.AreEqual("GetById", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(modelIn.Name, modelOut.Name);
        }

        [TestMethod]
        public void PutWrongFormatTest() {
            //Arrange
            var modelIn = new TeamModelIn()
            {
                Photo = "/MyResource/DreamTeam.png"
            };
            //We need to force the error in de ModelState
            controller.ModelState.AddModelError("", "Error");
            IActionResult result = controller.Put(2,modelIn);
            //Act
            BadRequestObjectResult createdResult = result as BadRequestObjectResult;
            //Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }
    }
}