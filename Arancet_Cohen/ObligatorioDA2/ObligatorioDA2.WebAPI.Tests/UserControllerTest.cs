using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void CreateValidUserTest()
        {
            //Arrange
            var modelIn = new UserModelIn() { Name = "James", Surname = "Hetfield", Username = "JHetfield63", Email = "JHetfield@gmail.com" };
            var controller = new UsersController();
            var result = controller.Post(modelIn);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            //Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetById", createdResult.RouteName);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(modelIn.Username, modelOut.Username);
        }

        [TestMethod]
        public void CreateFailedUserRequiredMailTest()
        {
            //Arrange
            var modelIn = new UserModelIn() {
                Name= "name",
                Surname="surname",
                Username ="username",
                Password="password"
            };
            var controller = new UsersController();
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
        public void CreateFailedUserRequiredPasswordTest()
        {
            //Arrange
            var modelIn = new UserModelIn()
            {
                Name = "name",
                Surname = "surname",
                Username = "username",
                Email = "email"
            };
            var controller = new UsersController();
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
        public void CreateFailedUserRequiredUsernameTest()
        {
            //Arrange
            var modelIn = new UserModelIn()
            {
                Name = "name",
                Surname = "surname",
                Password = "password",
                Email = "email"
            };
            var controller = new UsersController();
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
        public void CreateFailedUserRequiredNameTest()
        {
            //Arrange
            var modelIn = new UserModelIn()
            {
                Surname = "surname",
                Username= "username",
                Password = "password",
                Email = "email"
            };
            var controller = new UsersController();
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
        public void CreateFailedUserRequiredSurnameTest()
        {
            //Arrange
            var modelIn = new UserModelIn()
            {
                Name = "name",
                Username = "username",
                Password = "password",
                Email = "email"
            };
            var controller = new UsersController();
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
