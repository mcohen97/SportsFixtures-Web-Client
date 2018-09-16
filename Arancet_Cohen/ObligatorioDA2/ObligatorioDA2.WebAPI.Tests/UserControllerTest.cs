using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligatorioDA2.WebAPI.Models;

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
    }
}
