using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.WebAPI.Models;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
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
        public IActionResult Post([FromBody] TeamModelIn team)
        {
            IActionResult toReturn;
            if (ModelState.IsValid)
            {
                var addedTeam = new TeamModelOut() { 
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