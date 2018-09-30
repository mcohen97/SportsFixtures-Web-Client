using System;
using BusinessLogic;
using DataRepositoryInterfaces;
using ObligatorioDA2.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using System.Collections.Generic;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class UserServiceTest
    {
        private Mock<IUserRepository> users;
        private Mock<ITeamRepository> teams;
        private IUserService service;
        private User testUser;
        private Team toFollow;

        [TestInitialize]
        public void SetUp() {
            users = new Mock<IUserRepository>();
            teams = new Mock<ITeamRepository>();
            service = new UserService(users.Object,teams.Object);
            testUser = GetFakeUser();
            users.Setup(r => r.Get("JohnDoe")).Returns(testUser);
            users.Setup(r => r.Get(It.Is<string>(s => !s.Equals("JohnDoe")))).Throws(new UserNotFoundException());
             toFollow = GetFakeTeam();
        }

        private User GetFakeUser()
        {
            UserId identity = new UserId
            {
                Name = "John",
                Surname = "Doe",
                UserName = "JohnDoe",
                Password = "Password",
                Email = "John@Doe.com"
            };
            return new User(identity, true);
        }

        private Team GetFakeTeam()
        {
            Sport played = new Sport("Basketball");
            Team fake = new Team(1, "Lakers", "aPhoto", played);
            return fake;
        }

        [TestMethod]
        public void GetUserTest()
        {
            User retrieved = service.GetUser("JohnDoe");

            users.Verify(r => r.Get("JohnDoe"), Times.Once);
            Assert.AreEqual(retrieved, testUser);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void GetNotExistsTest() {
            User retrieved = service.GetUser("JohnLennon");
        }

        [TestMethod]
        public void AddUserTest()
        {
            service.AddUser(testUser);
            users.Verify(r => r.Add(testUser), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(UserAlreadyExistsException))]
        public void AddAlreadyExistentUserTest() {
            users.Setup(r => r.Add(testUser)).Throws(new UserAlreadyExistsException());
            service.AddUser(testUser);
        }

        [TestMethod]
        public void ModifyUserTest()
        {
            service.ModifyUser(testUser);
            users.Verify(r => r.Modify(testUser), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void ModifyNotExistentTest() {
            users.Setup(r => r.Modify(testUser)).Throws(new UserNotFoundException());
            service.ModifyUser(testUser);
        }

        [TestMethod]
        public void DeleteUserTest() {
            service.DeleteUser(testUser.UserName);
            users.Verify(r => r.Delete(testUser.UserName));
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void DeleteNotExistentUserTest()
        {
            users.Setup(r => r.Delete(testUser.UserName)).Throws(new UserNotFoundException());
            service.DeleteUser(testUser.UserName);
        }

        [TestMethod]
        public void FollowTeamtest() {
            service.FollowTeam(testUser.UserName, toFollow);

            users.Verify(r => r.Get(testUser.UserName), Times.Once);
            users.Verify(r => r.Modify(testUser), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void FollowTeamNotExistentUserTest() {
            users.Setup(r => r.Get(testUser.UserName)).Throws(new UserNotFoundException());

            service.FollowTeam(testUser.UserName, toFollow);
            users.Verify(r => r.Get(testUser.UserName), Times.Once);
            users.Verify(r => r.Modify(testUser), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void FollowNotExistentTeamTest() {
            users.Setup(r => r.Modify(testUser)).Throws(new TeamNotFoundException());

            service.FollowTeam(testUser.UserName, toFollow);

            users.Verify(r => r.Get(testUser.UserName), Times.Once);
            users.Verify(r => r.Modify(testUser), Times.Once);
        }

        [TestMethod]
        public void GetUserTeamsTest() {
            Team fake = GetFakeTeam();
            teams.Setup(r => r.GetFollowedTeams(testUser.UserName)).Returns(new List<Team>() { fake });

            ICollection<Team> userTeams = GetUserTeams(testUser.UserName);
            Assert.AreEqual(userTeams.Count, 1);
        }
    }
}
