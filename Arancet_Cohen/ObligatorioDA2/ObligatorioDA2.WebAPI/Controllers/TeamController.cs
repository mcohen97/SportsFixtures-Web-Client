using System;
using System.Collections.Generic;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
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
            IActionResult result;
            Team fetched;
            try {
                fetched =TryGetTeam(id);
                TeamModelOut transferObject = CreateModelOut(fetched);        
                result = Ok(transferObject);
            } catch (TeamNotFoundException e) {
                result = new NotFoundObjectResult(e.Message);
            }
            return result;
        }

        private Team TryGetTeam(int id)
        {
            return teams.Get(id);
        }

        private TeamModelOut CreateModelOut(Team fetched)
        {
            TeamModelOut toReturn = new TeamModelOut()
            {
                Id = fetched.Id,
                Name = fetched.Name,
                Photo = fetched.Photo
            };
            return toReturn;
        }


        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] TeamModelIn team)
        {
            IActionResult toReturn;
            if (ModelState.IsValid)
            {
                Team toAdd = new Team(team.Name, team.Photo);
                
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
        public IActionResult Put(int id, [FromBody] TeamModelIn value)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = PutValid(id, value);
            }
            else {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult PutValid(int id, TeamModelIn value)
        {
            IActionResult result;
            try
            {
                Team toModify = new Team(id,value.Name, value.Photo);
                teams.Modify(toModify);
                result = Ok();
            }
            catch(TeamNotFoundException) {
              result = PostId(id, value);
            }
            return result;
        }

        private IActionResult PostId(int id, TeamModelIn team)
        {
            Team toAdd = new Team(team.Name, team.Photo);

            teams.Add(toAdd);

            TeamModelOut addedTeam = new TeamModelOut()
            {
                Id = id,
                Name = team.Name,
                Photo = team.Photo
            };

            return CreatedAtRoute("GetById", new { id = addedTeam.Id }, addedTeam);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}