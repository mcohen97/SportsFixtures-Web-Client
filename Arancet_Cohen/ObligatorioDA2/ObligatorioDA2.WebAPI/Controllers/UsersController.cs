using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.WebAPI.Models;
using RepositoryInterface;
using BusinessLogic;
using BusinessLogic.Factories;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IRepository<User> repo;
        UserFactory factory;

        public UsersController(IRepository<User> aRepo) {
            repo = aRepo;
            factory = new UserFactory();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] UserModelIn user)
        {
            IActionResult toReturn;
            if (ModelState.IsValid)
            {
                var addedUser = new UserModelOut() { Id = 1, Username = user.Username, Name = user.Name,
                    Surname =user.Surname,Email=user.Email };
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
                toReturn = CreatedAtRoute("GetById", new { id = addedUser.Id }, addedUser);
            }
            else
            {
                toReturn = BadRequest(ModelState);
            }
            return toReturn;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
