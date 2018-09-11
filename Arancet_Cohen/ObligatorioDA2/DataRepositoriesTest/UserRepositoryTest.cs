using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataAccess;
using DataRepositoryInterfaces;
using DataRepositories;

namespace DataAccessTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        IUserRepository usersStorage;

        [TestInitialize]
        public void SetUp() {
            Mock<DatabaseConnection> connection = new Mock<DatabaseConnection>();
           
            //usersStorage = new UserRepository(connec);
        }
        [TestMethod]
        public void GetUserTest()
        {

        }
    }
}
