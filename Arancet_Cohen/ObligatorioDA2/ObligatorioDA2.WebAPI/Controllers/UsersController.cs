using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using System.Net;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService service;
        private UserFactory factory;


        public UsersController(IUserService aService) {
            service = aService;
            factory = new UserFactory();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            IActionResult result;
            try {
                result = TryGetAll();
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult TryGetAll()
        {
            ICollection<User> users = service.GetAllUsers();
            ICollection<UserModelOut> output = users.Select(u => CreateModelOut(u)).ToList();
            return Ok(output);
        }

        [HttpGet("{username}", Name = "GetUserById")]
        [Authorize]
        public IActionResult Get(string username)
        {
            IActionResult result;
            try
            {
                UserModelOut toReturn = TryGetUser(username);
                result = Ok(toReturn);
            }
            catch (UserNotFoundException e)
            {
                ErrorModelOut error = CreateErrorModel(e);
                result = new NotFoundObjectResult(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }


        private UserModelOut TryGetUser(string username) {
            User queried = service.GetUser(username);
            UserModelOut toReturn = new UserModelOut
            {
                Name = queried.Name,
                Surname = queried.Surname,
                Username = queried.UserName,
                Email = queried.Email
            };
            return toReturn;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody] UserModelIn user)
        {
            IActionResult toReturn;
            if (ModelState.IsValid)
            {
                toReturn = AddValidUser(user);
            }
            else
            {
                toReturn = BadRequest(ModelState);
            }
            return toReturn;
        }

        private IActionResult AddValidUser(UserModelIn user) {

            IActionResult result;
            try
            {
                result = TryAddUser(user);
            }
            catch (UserAlreadyExistsException e)
            {
                ErrorModelOut error = CreateErrorModel(e);
                result = BadRequest(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult TryAddUser(UserModelIn user)
        {
            User toAdd = BuildUser(user);
            service.AddUser(toAdd);
            UserModelOut modelOut = CreateModelOut(toAdd);
            return CreatedAtRoute("GetUserById", new { username = toAdd.UserName }, modelOut);
        }

        private ErrorModelOut CreateErrorModel(Exception e)
        {
            return new ErrorModelOut() { ErrorMessage = e.Message };
        }


        [HttpPut("{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(string username, [FromBody] UserModelIn toModify)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = ModifyValidUser(username, toModify);
            }
            else {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult ModifyValidUser(string id, UserModelIn input)
        {
            IActionResult result;
            User toModify = BuildUser(input);
            UserModelOut output = CreateModelOut(toModify);
            try
            {
                service.ModifyUser(toModify);
                result = Ok(output);
            }
            catch (UserNotFoundException)
            {
                service.AddUser(toModify);
                result = CreatedAtRoute("GetUserById", new { username = toModify.UserName }, output);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        [HttpDelete("{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string username)
        {
            IActionResult result;
            try
            {
                service.DeleteUser(username);
                result = Ok();
            }
            catch (UserNotFoundException e)
            {
                ErrorModelOut error = CreateErrorModel(e);
                result = new NotFoundObjectResult(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private User BuildUser(UserModelIn modelIn)
        {
            UserId identity = new UserId
            {
                Name = modelIn.Name,
                Surname = modelIn.Surname,
                UserName = modelIn.Username,
                Password = modelIn.Password,
                Email = modelIn.Email
            };
            User built = modelIn.IsAdmin ? factory.CreateAdmin(identity) : factory.CreateFollower(identity);
            return built;
        }

        [HttpPost, Route("followed-teams")]
        [Authorize]
        public IActionResult FollowTeam([FromBody] TeamModelIn aTeam)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = FollowValidFormatTeam(aTeam);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult FollowValidFormatTeam(TeamModelIn aTeam)
        {
            IActionResult result;
            try
            {
                result = TryFollowTeam(aTeam);
            }
            catch (EntityNotFoundException e1)
            {
                ErrorModelOut error = CreateErrorModel(e1);
                result = NotFound(error);
            }
            catch (TeamAlreadyFollowedException e2)
            {
                ErrorModelOut error = CreateErrorModel(e2);
                result = BadRequest(error);
            }
            return result;
        }

        private IActionResult TryFollowTeam(TeamModelIn aTeam)
        {
            IActionResult result;
            try
            {
                string username = HttpContext.User.Claims.First(c => c.Type.Equals("Username")).Value;
                service.FollowTeam(username, aTeam.Id);
                OkModelOut okMessage = new OkModelOut() { OkMessage = "You now follow the team" };
                result = Ok(okMessage);
            }
            catch (DataInaccessibleException e)
            {
                result = NoDataAccess(e);
            }
            return result;
        }

        [HttpDelete, Route("followed-teams")]
        [Authorize]
        public IActionResult UnFollowTeam(TeamModelIn aTeam)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = UnFollowValidFormat(aTeam);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult UnFollowValidFormat(TeamModelIn input)
        {
            IActionResult result;
            try
            {
                result = TryUnFollow(input);
            }
            catch (EntityNotFoundException e)
            {
                ErrorModelOut error = CreateErrorModel(e);
                result = NotFound(error);
            }
            catch (TeamNotFollowedException e)
            {
                ErrorModelOut error = CreateErrorModel(e);
                result = NotFound(error);
            }
            return result;
        }

        private IActionResult TryUnFollow(TeamModelIn input)
        {
            IActionResult result;
            try
            {
                string username = HttpContext.User.Claims.First(c => c.Type.Equals("Username")).Value;
                service.UnFollowTeam(username, input.Id);
                OkModelOut okMessage = new OkModelOut() { OkMessage = "Team unfollowed succesfully" };
                result = Ok(okMessage);
            }
            catch (DataInaccessibleException e)
            {
                result = NoDataAccess(e);
            }
            return result;
        }

        [HttpGet("{username}/followed-teams")]
        [Authorize]
        public IActionResult GetFollowedTeams(string username)
        {
            IActionResult result;
            try
            {
                result = TryGetFollowedTeams(username);
            }
            catch (UserNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = BadRequest(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult TryGetFollowedTeams(string username)
        {
            ICollection<Team> followed = service.GetUserTeams(username);
            ICollection<TeamModelOut> converted = followed.Select(t => CreateModelOut(t)).ToList();
            return Ok(converted);
        }

        private UserModelOut CreateModelOut(User added)
        {
            UserModelOut built = new UserModelOut()
            {
                Username = added.UserName,
                Name = added.Name,
                Surname = added.Surname,
                Email = added.Email
            };
            return built;
        }

        private TeamModelOut CreateModelOut(Team stored)
        {
            TeamModelOut built = new TeamModelOut()
            {
                Id= stored.Id,
                Name= stored.Name,
                SportName = stored.Sport.Name,
                Photo= stored.Photo
            };
            return built;
        }

        private IActionResult NoDataAccess(DataInaccessibleException e)
        {
            ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
            IActionResult internalError = StatusCode((int)HttpStatusCode.InternalServerError, error);
            return internalError;
        }

    }
}
