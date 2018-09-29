using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using DataRepositories;
using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class UserControllerTest
    {
        UsersController controller;
        UserRepository repo;
        UserModelIn input;

        [TestInitialize]
        public void SetUp()
        {

            InstantiateController();
            repo.Clear();
            input = new UserModelIn() { Name = "James", Surname = "Hetfield", Username = "JHetfield63", Password = "password", Email = "JHetfield@gmail.com" };
        }

        private void InstantiateController()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "UserRepository")
                .Options;

            repo = new UserRepository(new DatabaseConnection(options));
            controller = new UsersController(repo);
        }

        [TestMethod]
        public void GetTest()
        {
            IActionResult postResult = controller.Post(input);
            CreatedAtRouteResult createdResult = postResult as CreatedAtRouteResult;
            UserModelOut modelOut = createdResult.Value as UserModelOut;
            IActionResult fetchedById = controller.Get(modelOut.Username);
            OkObjectResult okResult = fetchedById as OkObjectResult;
            UserModelOut userData = okResult.Value as UserModelOut;
            Assert.AreEqual(modelOut.Username, userData.Username);
        }

        [TestMethod]
        public void GetNotExistentTest()
        {
            IActionResult fetchedById = controller.Get("username");
            NotFoundResult result = fetchedById as NotFoundResult;
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void CreateValidUserResultTest()
        {
            //Arrange

            var result = controller.Post(input);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            //Assert
            Assert.IsNotNull(createdResult);
        }

        [TestMethod]
        public void CreateValidUserCreaatedRouteTest()
        {
            //Arrange
            var result = controller.Post(input);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            //Assert
            Assert.AreEqual("GetById", createdResult.RouteName);
        }

        [TestMethod]
        public void CreateValidUserCodeTest()
        {
            //Arrange
            var result = controller.Post(input);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            //Assert

            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [TestMethod]
        public void CreateValidUserOutPutUsernameTest()
        {
            //Arrange
            var result = controller.Post(input);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            //Assert
            Assert.AreEqual(input.Username, modelOut.Username);
        }

        [TestMethod]
        public void CreateValidUserOutPutNameTest()
        {
            //Arrange
            var result = controller.Post(input);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            //Assert
            Assert.AreEqual(input.Name, modelOut.Name);
        }

        [TestMethod]
        public void CreateValidUserOutPutSurnameTest()
        {
            //Arrange
            var result = controller.Post(input);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            //Assert
            Assert.AreEqual(input.Surname, modelOut.Surname);
        }

        [TestMethod]
        public void CreateValidUserOutPutEmailTest()
        {
            //Arrange
            var result = controller.Post(input);

            //Act
            var createdResult = result as CreatedAtRouteResult;
            var modelOut = createdResult.Value as UserModelOut;

            //Assert
            Assert.AreEqual(input.Email, modelOut.Email);
        }

        [TestMethod]
        public void CreateFailedUserRequiredMailTest()
        {
            //Arrange
            var modelIn = new UserModelIn()
            {
                Name = "name",
                Surname = "surname",
                Username = "username",
                Password = "password"
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
                Username = "username",
                Password = "password",
                Email = "email"
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
        public void PutModifyTest()
        {
            var modelIn = new UserModelIn()
            {
                Name = "name1",
                Surname = "surname1",
                Username = "username",
                Password = "password1",
                Email = "mail@domain.com"
            };

            var modifiedModelIn = new UserModelIn()
            {
                Name = "name2",
                Surname = "surname2",
                Username = "username",
                Password = "password2",
                Email = "mail@domain.com"
            };

            CreatedAtRouteResult result = (CreatedAtRouteResult)controller.Post(modelIn);
            InstantiateController();
            UserModelOut created = (UserModelOut)result.Value;
            controller.Put(created.Username, modifiedModelIn);
            OkObjectResult getResult = controller.Get(created.Username) as OkObjectResult;
            UserModelOut updated = getResult.Value as UserModelOut;
            Assert.AreEqual("name2", updated.Name);
        }

        [TestMethod]
        public void PutCreateTest()
        {
            var modelIn = new UserModelIn()
            {
                Name = "name1",
                Surname = "surname1",
                Username = "username",
                Password = "password1",
                Email = "mail@domain.com"
            };

            controller.Put("username", modelIn);
            OkObjectResult getResult = controller.Get("username") as OkObjectResult;
            UserModelOut updated = getResult.Value as UserModelOut;
            Assert.AreEqual("name1", updated.Name);
        }

        [TestMethod]
        public void PutBadFormatTest() {
            var model = new UserModelIn()
            {
                Surname = "surname1",
                Password = "password1",
                Email = "mail@domain.com"
            };
            controller.ModelState.AddModelError("", "Error");
            IActionResult result =controller.Put("username", model);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
        }

        [TestMethod]
        public void DeleteTest()
        {
            CreatedAtRouteResult result = (CreatedAtRouteResult)controller.Post(input);
            UserModelOut created = (UserModelOut)result.Value;
            controller.Delete(created.Username);
            IActionResult fetchedById = controller.Get(created.Username);
            NotFoundResult wasDeleted = fetchedById as NotFoundResult;
            Assert.IsNotNull(wasDeleted);
        }

        [TestMethod]
        public void DeleteNotExistentTest()
        {
            NotFoundResult deleteResult = controller.Delete("notExistent") as NotFoundResult;
            Assert.IsNotNull(deleteResult);
        }
    }
}