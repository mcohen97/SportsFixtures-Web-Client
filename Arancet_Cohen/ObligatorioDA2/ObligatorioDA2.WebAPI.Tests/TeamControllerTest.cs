using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RepositoryInterface;
using BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using DataRepositoryInterfaces;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class TeamControllerTest
    {
        private TeamsController controller;
        private Mock<ITeamRepository> repo;
        Team team;

        [TestInitialize]
        public void SetUp() {
            team = new Team(2,"Nacional", "aPath");
            repo = new Mock<ITeamRepository>();
            repo.Setup(r => r.Get("Soccer","Nacional")).Returns(team);
            repo.Setup(r => r.Get(It.Is<string>(i => !i.Equals("Soccer")), It.Is<string>(i => !i.Equals("Nacional"))))
                .Throws(new TeamNotFoundException());
            controller = new TeamsController(repo.Object);
        }

        [TestMethod]
        public void CreateValidTeamTest()
        {
            //Arrange
            var modelIn = new TeamModelIn() {
                 Name = "DreamTeam", 
                 Photo = "/MyResource/DreamTeam.png",
                 SportName = "Soccer"
            };
            var result = controller.Post(modelIn);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as TeamModelOut;

            //Assert
            repo.Verify(r => r.Add(It.Is<string>(i => i.Equals("Soccer")),It.IsAny<Team>()), Times.Once);
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

            IActionResult result = controller.Get("Soccer","DreamTeam") as IActionResult;
            OkObjectResult okResult = result as OkObjectResult;
            TeamModelOut resultTeam = okResult.Value as TeamModelOut;

            repo.Verify(r => r.Get("Soccer", "DreamTeam"), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(200,okResult.StatusCode);
            Assert.AreEqual(resultTeam.Name, team.Name);
        }

        [TestMethod]
        public void GetTeamNotFoundTest() {
            IActionResult result = controller.Get("Basketball", "DreamTeam") as IActionResult;
            NotFoundObjectResult notFoundResult = result as NotFoundObjectResult;

            repo.Verify(r => r.Get("Basketball", "DreamTeam"), Times.Once);
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, 404);
        }

        [TestMethod]
        public void PutTest() {
            //Arrange
            var modelIn = new TeamModelIn()
            {
                Name = "DreamTeam",
                Photo = "/MyResource/DreamTeam.png",
                SportName = "Soccer"
            };

            IActionResult result = controller.Put("Soccer",modelIn);
            OkResult okResult = result as OkResult;

            //verify it modifies but not adds
            repo.Verify(r => r.Modify("Soccer",It.IsAny<Team>()), Times.Once);
            repo.Verify(r => r.Add("Soccer",It.IsAny<Team>()), Times.Never);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void PutAddTest() {

            //make repository throw not existing exception, so that it has to add
            repo.Setup(r => r.Modify(It.IsAny<string>(),It.IsAny<Team>())).Throws(new TeamNotFoundException());

            var modelIn = new TeamModelIn()
            {
                Name = "DreamTeam",
                Photo = "/MyResource/DreamTeam.png",
                SportName= "Soccer"
                
            };

            //act
            IActionResult result =controller.Put("Soccer",modelIn);
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as TeamModelOut;

            //assert
            repo.Verify(r => r.Modify("Soccer",It.IsAny<Team>()), Times.Once);
            repo.Verify(r => r.Add("Soccer",It.IsAny<Team>()), Times.Once);
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
            IActionResult result = controller.Put("Soccer",modelIn);
            //Act
            BadRequestObjectResult createdResult = result as BadRequestObjectResult;
            //Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }

        [TestMethod]
        public void DeleteTest() {

            IActionResult result =controller.Delete("Soccer","Nacional");
            OkResult okResult = result as OkResult;

            repo.Verify(r => r.Delete("Soccer", "Nacional"), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void DeleteNotExistentTest() {
            repo.Setup(r => r.Delete(It.IsAny<string>(),It.IsAny<string>())).Throws(new TeamNotFoundException());

            IActionResult result = controller.Delete("Soccer", "Nacional");
            BadRequestObjectResult okResult = result as BadRequestObjectResult;

            repo.Verify(r => r.Delete("Soccer", "Nacional"), Times.Once);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(okResult.StatusCode, 400);
        }
    }
}