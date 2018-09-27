using System.Collections.Generic;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.WebAPI.Models;
using RepositoryInterface;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private IRepository<Team> teams;
        public TeamController(IRepository<Team> aRepo) {
            teams=aRepo;
        }
          // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] TeamModelIn team)
        {
            IActionResult toReturn;
            if (ModelState.IsValid)
            {
                Team toAdd = new Team(team.Name, "");
                
                teams.Add(toAdd);

                TeamModelOut addedTeam = new TeamModelOut()
                {
                    Id = 1,
                    Name = team.Name,
                    Photo = team.Photo
                };

                toReturn = CreatedAtRoute("GetById", new { id = addedTeam.Id }, addedTeam);
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