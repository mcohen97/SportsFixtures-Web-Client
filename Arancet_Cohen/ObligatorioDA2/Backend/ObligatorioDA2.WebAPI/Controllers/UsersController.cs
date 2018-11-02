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
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        private UserFactory factory;
        private IImageService images;
        private ErrorActionResultFactory errors;


        public UsersController(IUserService aService, IImageService imageService) {
            userService = aService;
            images = imageService;
            factory = new UserFactory();
            errors = new ErrorActionResultFactory(this);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            IActionResult result;
            try {
                result = TryGetAll();
            }
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryGetAll()
        {
            ICollection<User> users = userService.GetAllUsers();
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
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private UserModelOut TryGetUser(string username) {
            User queried = userService.GetUser(username);
            UserModelOut toReturn = new UserModelOut
            {
                Name = queried.Name,
                Surname = queried.Surname,
                Username = queried.UserName,
                Email = queried.Email,
                IsAdmin = queried.IsAdmin
            };
            return toReturn;
        }

        [HttpPost]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
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
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }

            return result;
        }

        private IActionResult TryAddUser(UserModelIn user)
        {

            UserDto toAdd = BuildUser(user);
            User added =userService.AddUser(toAdd);
            UserModelOut modelOut = CreateModelOut(added);
            return CreatedAtRoute("GetUserById", new { username = toAdd.username }, modelOut);
        }

        [HttpPut("{username}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Put(string username, [FromBody] UpdateUserModelIn input)
        {
            IActionResult result;
            UserDto toModify = BuildUser(username, input);
            try
            {
                User modified =userService.ModifyUser(toModify);
                result = Ok(CreateModelOut(modified));
            }
            catch (ServiceException e)
            {
                if (e.Error.Equals(ErrorType.ENTITY_NOT_FOUND))
                {
                    User added = userService.AddUser(toModify);
                    result = CreatedAtRoute("GetUserById", new { username = toModify.username }, CreateModelOut(added));
                }
                else
                {
                    result = errors.GenerateError(e);
                }
            }
            return result;
        }

        [HttpDelete("{username}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Delete(string username)
        {
            IActionResult result;
            try
            {
                userService.DeleteUser(username);
                OkModelOut okMessage = new OkModelOut() { OkMessage = "The user has been deleted successfully" };
                result = Ok(okMessage);
            }
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }

            return result;
        }
        private UserDto BuildUser(string username,UpdateUserModelIn modelIn)
        {
            UserDto built = new UserDto()
            {
                name = modelIn.Name,
                surname = modelIn.Surname,
                username = username,
                password = modelIn.Password,
                email = modelIn.Email,
                isAdmin = modelIn.IsAdmin
            };
            return built;
        }

        private UserDto BuildUser(UserModelIn modelIn)
        {
            UserDto built = new UserDto()
            {
                name = modelIn.Name,
                surname = modelIn.Surname,
                username = modelIn.Username,
                password = modelIn.Password,
                email = modelIn.Email,
                isAdmin = modelIn.IsAdmin
            };
            return built;
        }

        [HttpPost, Route("followed-teams/{teamId}")]
        [Authorize]
        public IActionResult FollowTeam(int teamId)
        {
            IActionResult result;
            try
            {
                result = TryFollowTeam(teamId);
            }
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryFollowTeam(int teamId)
        {
           IActionResult result;
           string username = HttpContext.User.Claims.First(c => c.Type.Equals(AuthenticationConstants.USERNAME_CLAIM)).Value;
           userService.FollowTeam(username, teamId);
           OkModelOut okMessage = new OkModelOut() { OkMessage = "You now follow the team" };
           result = Ok(okMessage);

            return result;
        }

        [HttpDelete, Route("followed-teams/{teamId}")]
        [Authorize]
        public IActionResult UnFollowTeam(int teamId)
        {
            return UnFollowValidFormat(teamId);
        }

        private IActionResult UnFollowValidFormat(int teamId)
        {
            IActionResult result;
            try
            {
                result = TryUnFollow(teamId);
            }
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryUnFollow(int teamId)
        {
            IActionResult result;
            string username = HttpContext.User.Claims.First(c => c.Type.Equals(AuthenticationConstants.USERNAME_CLAIM)).Value;
            userService.UnFollowTeam(username, teamId);
            OkModelOut okMessage = new OkModelOut() { OkMessage = "Team unfollowed succesfully" };
            result = Ok(okMessage);
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
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryGetFollowedTeams(string username)
        {
            ICollection<Team> followed = userService.GetUserTeams(username);
            ICollection<TeamModelOut> converted = followed.Select(t => CreateModelOut(t)).ToList();
            return Ok(converted);
        }

        private UserModelOut CreateModelOut(User user)
        {
            UserModelOut built = new UserModelOut()
            {
                Username = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };
            return built;
        }

        private TeamModelOut CreateModelOut(Team stored)
        {
            TeamModelOut built = new TeamModelOut()
            {
                Id = stored.Id,
                Name = stored.Name,
                SportName = stored.Sport.Name,
                Photo = images.ReadImage(stored.PhotoPath)
            };
            return built;
        }
    }
}
