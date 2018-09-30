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

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRepository users;
        ITeamRepository teams;
        UserFactory factory;


        public UsersController(IUserRepository usersRepo, ITeamRepository teamsRepo) {
            users = usersRepo;
            teams = teamsRepo;
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
                result = new NotFoundResult();
            }
            return result;
        }

        private UserModelOut TryGetUser(string username) {
            User queried = users.Get(username);
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

        [HttpPost("{username}/teams")]
        public void Post(string username,TeamModelIn modelIn) {
            Team toFollow = new Team(modelIn.Name);

            users.AddFollowedTeam(username,modelIn.SportName,toFollow);
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
            users.Add(toAdd);
            User added = users.Get(toAdd); 
           
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
                users.Modify(converted);
            }
            catch(UserNotFoundException) {
                users.Add(converted);
            }

        }

        [HttpDelete("{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string username)
        {
            IActionResult result;
            try
            {
                users.Delete(username);
                result = Ok();
            }
            catch (UserNotFoundException e) {
                result = new NotFoundResult();
            }
            return result;
        }
    }
}
