using ObligatorioDA2.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.WebAPI.Controllers;
using ObligatorioDA2.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Match = ObligatorioDA2.BusinessLogic.Match;
using System.Linq;

namespace ObligatorioDA2.WebAPI.Tests
{
    [TestClass]
    public class MatchesControllerTest{

        private Mock<IMatchService> matchService;
        private MatchesController controller;
        private Match testMatch;

        [TestInitialize]
        public void SetUp() {
            matchService = new Mock<IMatchService>();
            testMatch = BuildFakeMatch();
            controller = new MatchesController(matchService.Object);
        }

        private Match BuildFakeMatch()
        {
            Sport played = new Sport("Football",true);
            Team home = new Team("Patriots", "aPhoto", played);
            Team away = new Team("Falcons", "aPhoto", played);
            Match built = new Match(1,new List<Team>() { home, away }, DateTime.Now, played);
            return built;
        }

        [TestMethod]
        public void GetTest()
        {
            //Arrange.
            matchService.Setup(s => s.GetAllMatches()).Returns(new List<Match>() { testMatch });

            //Act.
            IActionResult result = controller.Get();
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<MatchModelOut> matches = okResult.Value as ICollection<MatchModelOut>;

            //Assert.
            matchService.Verify(s => s.GetAllMatches(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(matches);
            Assert.AreEqual(matches.Count, 1);
        }

        [TestMethod]
        public void CreateValidMatchTest() {
            matchService.Setup(ms => ms.AddMatch(It.IsAny<ICollection<int>>(),
                It.IsAny<string>(), It.IsAny<DateTime>())).Returns(testMatch);
            MatchModelIn input = BuildMatchModelIn(testMatch);

            IActionResult result = controller.Post(input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            MatchModelOut created = createdResult.Value as MatchModelOut;

            matchService.Verify(ms => ms.AddMatch(It.IsAny<ICollection<int>>(),
                It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual("GetMatchById", createdResult.RouteName);
            Assert.IsNotNull(created);
            Assert.AreEqual(testMatch.Date, testMatch.Date);
        }

        private MatchModelIn BuildMatchModelIn(Match aMatch)
        {
            MatchModelIn built = new MatchModelIn()
            {
                SportName = aMatch.Sport.Name,
                TeamIds = aMatch.GetParticipants().Select(p => p.Id).ToList(),
                Date = aMatch.Date
            };
            return built;
        }

        [TestMethod]
        public void CreateInvalidMatchTest() {
            //Arrange.
            MatchModelIn input = new MatchModelIn() { };
            //We need to force the error in de ModelState.
            controller.ModelState.AddModelError("", "Error");

            //Act.
            IActionResult result = controller.Post(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;

            matchService.Verify(ms => ms.AddMatch(testMatch), Times.Never);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void CreateMatchSameDayForTeamTest() {
            //Arrange.
            Exception toThrow = new TeamAlreadyHasMatchException();
            matchService.Setup(ms => ms.AddMatch(It.IsAny<ICollection<int>>(),
                It.IsAny<string>(), It.IsAny<DateTime>())).Throws(toThrow);
            MatchModelIn input = BuildMatchModelIn(testMatch);

            //Act.
            IActionResult result = controller.Post(input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;


            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage,toThrow.Message);
        }

        [TestMethod]
        public void GetMatchTest() {
            //Arrange.
            matchService.Setup(ms => ms.GetMatch(It.IsAny<int>())).Returns(testMatch);

            //Act.
            IActionResult result =controller.Get(1);
            OkObjectResult foundResult = result as OkObjectResult;
            MatchModelOut match = foundResult.Value as MatchModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
            Assert.IsNotNull(match);
            Assert.AreEqual(1, match.Id);
        }

        [TestMethod]
        public void GetNotExistingMatchTest() {
            //Arrange.
            Exception toThrow = new MatchNotFoundException();
            matchService.Setup(ms => ms.GetMatch(It.IsAny<int>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.Get(1);
            BadRequestObjectResult notFoundresult = result as BadRequestObjectResult;
            ErrorModelOut error = notFoundresult.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFoundresult);
            Assert.AreEqual(400, notFoundresult.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void PutModifyMatchTest() {
            //Arrange.
            MatchModelIn input = BuildMatchModelIn(testMatch);

            //Act.
            IActionResult result = controller.Put(1, input);
            OkObjectResult okResult = result as OkObjectResult;
            MatchModelOut modified = okResult.Value as MatchModelOut;

            //Assert.
            matchService.Verify(ms => ms.ModifyMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            matchService.Verify(ms => ms.AddMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(modified);
            Assert.AreEqual(modified.Id, testMatch.Id);
        }

        [TestMethod]
        public void PutAdd() {
            //Arrange.
            matchService.Setup(ms => ms.ModifyMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<DateTime>(),It.IsAny<string>())).Throws(new MatchNotFoundException());
            matchService.Setup(ms => ms.AddMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<string>(), It.IsAny<DateTime>())).Returns(testMatch);

            MatchModelIn input = BuildMatchModelIn(testMatch);

            //Act.
            IActionResult result = controller.Put(1, input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            MatchModelOut modified = createdResult.Value as MatchModelOut;

            //Assert.
            matchService.Verify(ms => ms.ModifyMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            matchService.Verify(ms => ms.AddMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual("GetMatchById", createdResult.RouteName);
            Assert.IsNotNull(modified);
            Assert.AreEqual(modified.Id, testMatch.Id);
        }

        [TestMethod]
        public void PutNotValidTest() {
            //Arrange.
            MatchModelIn modelIn = new MatchModelIn() { };
            //We need to force the error in de ModelState.
            controller.ModelState.AddModelError("", "Error");

            //Act.
            IActionResult result = controller.Put(1, modelIn);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;

            //Assert.
            matchService.Verify(ms => ms.ModifyMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);

            matchService.Verify(ms => ms.AddMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);

            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void PutDateOccupiedTest() {
            //Arrange.
            Exception toThrow = new TeamAlreadyHasMatchException();
            matchService.Setup(ms => ms.ModifyMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<DateTime>(), It.IsAny<string>())).Throws(toThrow);
            MatchModelIn input = BuildMatchModelIn(testMatch);

            //Act.
            IActionResult result = controller.Put(1, input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert.
            matchService.Verify(ms => ms.ModifyMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);

            matchService.Verify(ms => ms.AddMatch(It.IsAny<int>(), It.IsAny<ICollection<int>>(),
                It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void DeleteTest() {
            //Act.
            IActionResult result =controller.Delete(3);
            OkObjectResult okResult = result as OkObjectResult;

            //Assert.
            matchService.Verify(ms => ms.DeleteMatch(3), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public void DeleteNotExistentTest() {
            //Arrange.
            Exception toThrow = new MatchNotFoundException();
            matchService.Setup(ms => ms.DeleteMatch(3)).Throws(toThrow);

            //Act.
            IActionResult result = controller.Delete(3);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.IsNotNull(error);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.AreEqual(error.ErrorMessage,toThrow.Message);
        }

        [TestMethod]
        public void CommentOnMatchTest() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            User commentarist = GetFakeUser();
            Commentary made = new Commentary("this is a comment", commentarist);

            CommentModelIn input = new CommentModelIn() {
                Text = "this is a comment"
            };
            matchService.Setup(ms => ms.CommentOnMatch(3, "username", input.Text)).Returns(made);


            //Act.
            IActionResult result = controller.CommentOnMatch(3,input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            CommentModelOut comment = createdResult.Value as CommentModelOut;

            //Assert.
            matchService.Verify(ms => ms.CommentOnMatch(3, "username", input.Text),Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual("GetCommentById", createdResult.RouteName);
            Assert.IsNotNull(comment);
            Assert.AreEqual(comment.Text, input.Text);
        }

        [TestMethod]
        public void CommentWithBadFormatTest() {
            //Arrange.
            CommentModelIn input = new CommentModelIn()
            {
                Text = "this is a comment",
            };
            controller.ModelState.AddModelError("", "Error");

            //Act.
            IActionResult result = controller.CommentOnMatch(3,input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;

            //Assert.
            matchService.Verify(ms => ms.CommentOnMatch(It.IsAny<int>(), It.IsAny<string>(), input.Text), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
        }

        [TestMethod]
        public void CreateCommentInNoExistingMatch() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            CommentModelIn input = new CommentModelIn()
            {
                Text = "this is a comment"
            };
            Exception toThrow = new MatchNotFoundException();
            matchService.Setup(ms => ms.CommentOnMatch(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.CommentOnMatch(3,input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert.
            matchService.Verify(ms => ms.CommentOnMatch(3, "username", input.Text), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void CreateCommentByNoExistingUser() {
            //Arrange.
            ControllerContext fakeContext = GetFakeControllerContext();
            controller.ControllerContext = fakeContext;
            CommentModelIn input = new CommentModelIn()
            {
                Text = "this is a comment"
            };
            Exception toThrow = new UserNotFoundException();
            matchService.Setup(ms => ms.CommentOnMatch(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.CommentOnMatch(3,input);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            ErrorModelOut error = badRequest.Value as ErrorModelOut;

            //Assert.
            matchService.Verify(ms => ms.CommentOnMatch(3, "username", input.Text), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(400, badRequest.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);

        }

        [TestMethod]
        public void ViewAllMatchCommentsTest() {
            //Arrange.
            User dummyUser = GetFakeUser();
            Commentary dummyComment = new Commentary("Comment", dummyUser);
            ICollection<Commentary> fakeList = new List<Commentary>() { dummyComment, dummyComment, dummyComment };
            matchService.Setup(ms => ms.GetMatchCommentaries(It.IsAny<int>())).Returns(fakeList);

            //Act.
            IActionResult result = controller.GetMatchComments(3);
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<CommentModelOut> comments = okResult.Value as ICollection<CommentModelOut>;

            //Assert.
            matchService.Verify(ms => ms.GetMatchCommentaries(It.IsAny<int>()),Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(comments);
            Assert.AreEqual(fakeList.Count, comments.Count);
        }

       [TestMethod]
        public void ViewCommentTest() {
            //Arrange.
            User dummyUser = GetFakeUser();
            Commentary dummyComment = new Commentary("Comment", dummyUser);
            matchService.Setup(ms => ms.GetComment(3)).Returns(dummyComment);

            //Act.
            IActionResult result = controller.GetComment(3);
            OkObjectResult okResult = result as OkObjectResult;
            CommentModelOut comment = okResult.Value as CommentModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(comment);
            Assert.AreEqual(dummyComment.Text,comment.Text);
        }

        [TestMethod]
        public void ViewNotExistingCommentTest() {
            //Arrange.
            Exception toThrow = new CommentNotFoundException();
            matchService.Setup(ms => ms.GetComment(3)).Throws(toThrow);

            //Act.
            IActionResult result = controller.GetComment(3);
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(error.ErrorMessage, toThrow.Message);
        }

        [TestMethod]
        public void ViewAllTheCommentsTest() {
            //Arrange.
            User dummyUser = GetFakeUser();
            Commentary dummyComment = new Commentary("Comment", dummyUser);
            ICollection<Commentary> fakeList = new List<Commentary>() { dummyComment, dummyComment, dummyComment };
            matchService.Setup(ms => ms.GetAllCommentaries()).Returns(fakeList);

            //Act.
            IActionResult result = controller.GetAllComments();
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<CommentModelOut> comments =okResult.Value as ICollection<CommentModelOut>;

            //Do.
            matchService.Verify(ms => ms.GetAllCommentaries(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(comments);
            Assert.AreEqual(fakeList.Count, comments.Count);
        }

        [TestMethod]
        public void GetSportMatchesTest() {
            //Arrange.
            ICollection<Match> dummies = new List<Match>() { testMatch, testMatch, testMatch };
            matchService.Setup(ms => ms.GetAllMatches(It.IsAny<string>())).Returns(dummies);

            //Act.
            IActionResult result = controller.GetBySport("Soccer");
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<MatchModelOut> matches = okResult.Value as ICollection<MatchModelOut>;

            //Assert.
            matchService.Verify(ms => ms.GetAllMatches(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(matches);
            Assert.AreEqual(dummies.Count, matches.Count);
        }

        [TestMethod]
        public void GetSportMatchesNotFoundTest() {
            //Arrange.
            Exception toThrow = new SportNotFoundException();
            matchService.Setup(ms => ms.GetAllMatches(It.IsAny<string>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.GetBySport("Soccer");
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            matchService.Verify(ms => ms.GetAllMatches(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        [TestMethod]
        public void GetTeamMatchesTest() {
            //Arrange.
            ICollection<Match> dummies = new List<Match>() { testMatch, testMatch, testMatch };
            matchService.Setup(ms => ms.GetAllMatches(It.IsAny<int>())).Returns(dummies);

            //Act.
            IActionResult result = controller.GetByTeam(5);
            OkObjectResult okResult = result as OkObjectResult;
            ICollection<MatchModelOut> matches = okResult.Value as ICollection<MatchModelOut>;

            //Assert.
            matchService.Verify(ms => ms.GetAllMatches(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(matches);
            Assert.AreEqual(dummies.Count, matches.Count);

        }

        [TestMethod]
        public void GetTeamMatchesNotFoundTest()
        {
            //Arrange.
            Exception toThrow = new TeamNotFoundException();
            matchService.Setup(ms => ms.GetAllMatches(It.IsAny<int>())).Throws(toThrow);

            //Act.
            IActionResult result = controller.GetByTeam(5);
            NotFoundObjectResult notFound = result as NotFoundObjectResult;
            ErrorModelOut error = notFound.Value as ErrorModelOut;

            //Assert.
            matchService.Verify(ms => ms.GetAllMatches(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
            Assert.IsNotNull(error);
            Assert.AreEqual(toThrow.Message, error.ErrorMessage);
        }

        private User GetFakeUser()
        {
            UserId identity = new UserId() { Name = "name", Surname = "surname", UserName = "username", Password = "password", Email = "email@email.com" };
            return new User(identity, true);
        }

        private ControllerContext GetFakeControllerContext()
        {
            ICollection<Claim> fakeClaims = new List<Claim>() { new Claim("Username", "username") };

            Mock<ClaimsPrincipal> cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.Claims).Returns(fakeClaims);
            Mock<HttpContext> contextMock = new Mock<HttpContext>();
            contextMock.Setup(ctx => ctx.User).Returns(cp.Object);

            Mock<ControllerContext> controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.Object.HttpContext = contextMock.Object;
            return controllerContextMock.Object;
        }
    }
   
}
