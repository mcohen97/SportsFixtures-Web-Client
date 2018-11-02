﻿using System;
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
            userService.AddUser(toAdd);
            UserModelOut modelOut = CreateModelOut(toAdd);
            return CreatedAtRoute("GetUserById", new { username = toAdd.username }, modelOut);
        }

        private ErrorModelOut CreateErrorModel(Exception e)
        {
            return new ErrorModelOut() { ErrorMessage = e.Message };
        }

        [HttpPut("{username}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
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
            UserDto toModify = BuildUser(input);
            UserModelOut output = CreateModelOut(toModify);
            try
            { 
                userService.ModifyUser(toModify);
                result = Ok(output);
            }
            catch (ServiceException e)
            {
                if (e.Error.Equals(ErrorType.ENTITY_NOT_FOUND))
                {
                    userService.AddUser(toModify);
                    result = CreatedAtRoute("GetUserById", new { username = toModify.username }, output);
                }
                else {
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
          return FollowValidFormatTeam(teamId);
        }

        private IActionResult FollowValidFormatTeam(int teamId)
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

        private UserModelOut CreateModelOut(UserDto added)
        {
            UserModelOut built = new UserModelOut()
            {
                Username = added.username,
                Name = added.name,
                Surname = added.surname,
                Email = added.email,
                IsAdmin = added.isAdmin
            };
            return built;
        }

        private UserModelOut CreateModelOut(User added)
        {
            UserModelOut built = new UserModelOut()
            {
                Username = added.UserName,
                Name = added.Name,
                Surname = added.Surname,
                Email = added.Email,
                IsAdmin = added.IsAdmin
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
