using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class TeamControllerTest
    {
        [TestMethod]
        public void CreateValidTeamTest()
        {
            //Arrange
            var modelIn = new TeamModelIn() {
                 Name = "DreamTeam", 
                 Photo = "/MyResource/DreamTeam.png"
            };
            var controller = new TeamController();
            var result = controller.Post(modelIn);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as TeamModelOut;

            //Assert
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
            var controller = new TeamController();
            //We need to force the error in de ModelState
            controller.ModelState.AddModelError("", "Error");
            var result = controller.Post(modelIn);
            //Act
            var createdResult = result as BadRequestObjectResult;
            //Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(400, createdResult.StatusCode);
        }


    }
}