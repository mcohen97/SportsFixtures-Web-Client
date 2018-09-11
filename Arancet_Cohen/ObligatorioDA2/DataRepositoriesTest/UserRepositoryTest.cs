using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataAccess;

namespace DataAccessTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        IUserRepository usersStorage;

        [TestInitialize]
        public void SetUp() {
            Mock<DatabaseConnection> connection = new Mock<DatabaseConnection>();
            usersStorage = new UserRepository();
        }
        [TestMethod]
        public void GetUserTest()
        {

        }
    }
}
