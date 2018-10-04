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

        [HttpGet]
        public IActionResult Get()
        {
            ICollection<User> users =service.GetAllUsers();
            ICollection<UserModelOut> output = users.Select(u => CreateModelOut(u)).ToList();
            return Ok(output);
        }

        [HttpGet("{username}", Name = "GetUserById")]
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

            IActionResult result;
            try
            {
                result = TryAddUser(user);
            }
            catch (UserAlreadyExistsException e) {
                ErrorModelOut error = CreateErrorModel(e);
                result = BadRequest(error);
            }
            return result;
        }

        private IActionResult TryAddUser(UserModelIn user)
        {
            User toAdd = BuildUser(user);
            service.AddUser(toAdd);
            UserModelOut modelOut = CreateModelOut(toAdd);
            return CreatedAtRoute("GetUserById", modelOut);
        }

        [HttpPut("{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(string username, [FromBody] UserModelIn toModify)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result =ModifyValidUser(username, toModify);
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
            catch(UserNotFoundException) {
                service.AddUser(toModify);
                result = CreatedAtRoute("GetUserById", output);
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
            catch (UserNotFoundException e) {
                ErrorModelOut error = CreateErrorModel(e);
                result = new NotFoundObjectResult(error);
            }
            return result;
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

    }
}
