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

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserRepository repo;
        UserFactory factory;

        public UsersController(IUserRepository aRepo) {
            repo = aRepo;
            factory = new UserFactory();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IActionResult result;
            try
            {
                UserModelOut toReturn = TryGetUser(id);
                result= Ok(toReturn);
            }catch (UserNotFoundException e) {
                result = new NotFoundResult();
            }
            return result;
        }

        private UserModelOut TryGetUser(int id) {
            User queried = repo.Get(id);
            UserModelOut toReturn = new UserModelOut
            {
                Id = queried.Id,
                Name = queried.Name,
                Surname = queried.Surname,
                Username = queried.UserName,
                Email = queried.Email
            };
            return toReturn;
        }

        // POST api/values
        [HttpPost]
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

            User toAdd = factory.CreateAdmin(identity);
            repo.Add(toAdd);
            User added = repo.Get(toAdd); 
           
            var addedUser = new UserModelOut()
            {
                Username = added.UserName,
                Name = added.Name,
                Surname = added.Surname,
                Email = added.Email,
                Id = added.Id
            };

            return CreatedAtRoute("GetById", new { id = added.Id }, addedUser);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserModelIn toModify)
        {
            IActionResult toReturn;
            if (ModelState.IsValid)
            {
                ModifyValidUser(id, toModify);
                toReturn = Ok();
            }
            else {
                toReturn = BadRequest(ModelState);
            }
            return toReturn;
        }

        private void ModifyValidUser(int id, UserModelIn toModify)
        {
            UserId identity = new UserId
            {
                Name = toModify.Name,
                Surname = toModify.Surname,
                UserName = toModify.Username,
                Password = toModify.Password,
                Email = toModify.Email
            };
            User converted = factory.CreateFollower(identity,id);
            try
            {
                repo.Modify(converted);
            }
            catch(UserNotFoundException) {
                repo.Add(converted);
            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            IActionResult result;
            try
            {
                repo.Delete(id);
                result = Ok();
            }
            catch (UserNotFoundException e) {
                result = new NotFoundResult();
            }
            return result;
        }
    }
}
