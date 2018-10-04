using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.WebAPI.Models;
using RepositoryInterface;
using BusinessLogic;
using BusinessLogic.Factories;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ObligatorioDA2.Services.Interfaces;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService service;
        UserFactory factory;


        public UsersController(IUserService aService) {
            service = aService;
            factory = new UserFactory();
        }

      
        [HttpGet("{username}", Name = "GetById")]
        public IActionResult Get(string username)
        {
            IActionResult result;
            try
            {
                UserModelOut toReturn = TryGetUser(username);
                result= Ok(toReturn);
            }catch (UserNotFoundException e) {
                ErrorModelOut error = CreateErrorModel(e);
                result = new NotFoundObjectResult(error);
            }
            return result;
        }

        private ErrorModelOut CreateErrorModel(Exception e)
        {
            return new ErrorModelOut() { ErrorMessage = e.Message };
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

        [HttpPost, Route("teams")]
        [Authorize]
        public IActionResult FollowTeam([FromBody] TeamModelIn aTeam) {
           string username = HttpContext.User.Claims.First(c => c.Type.Equals("Username")).Value;
            Team toFollow = new Team(aTeam.Id, aTeam.Name, aTeam.Photo, new Sport(aTeam.SportName));
            service.FollowTeam(username,toFollow);
            return Ok();
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
            UserId identity = new UserId
            {
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.Username,
                Password = user.Password,
                Email = user.Email
            };

            User toAdd =user.IsAdmin ? factory.CreateAdmin(identity):factory.CreateFollower(identity);
            service.AddUser(toAdd);
            User added = service.GetUser(toAdd.UserName); 
           
            var addedUser = new UserModelOut()
            {
                Username = added.UserName,
                Name = added.Name,
                Surname = added.Surname,
                Email = added.Email
            };

            return CreatedAtRoute("GetById", new { username = added.UserName }, addedUser);
        }

        [HttpPut("{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(string username, [FromBody] UserModelIn toModify)
        {
            IActionResult toReturn;
            if (ModelState.IsValid)
            {
                ModifyValidUser(username, toModify);
                toReturn = Ok();
            }
            else {
                toReturn = BadRequest(ModelState);
            }
            return toReturn;
        }

        private void ModifyValidUser(string id, UserModelIn toModify)
        {
            UserId identity = new UserId
            {
                Name = toModify.Name,
                Surname = toModify.Surname,
                UserName = toModify.Username,
                Password = toModify.Password,
                Email = toModify.Email
            };
            User converted = factory.CreateFollower(identity);
            try
            {
                service.ModifyUser(converted);
            }
            catch(UserNotFoundException) {
                service.AddUser(converted);
            }

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
            catch (UserNotFoundException e) {
                result = new NotFoundResult();
            }
            return result;
        }
    }
}
