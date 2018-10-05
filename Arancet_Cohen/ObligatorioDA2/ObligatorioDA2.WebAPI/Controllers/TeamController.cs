using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.WebAPI.Models;
using RepositoryInterface;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private ITeamRepository teams;
        public TeamsController(ITeamRepository aRepo)
        {
            teams = aRepo;


        }
        [HttpGet]
        public IActionResult Get()
        {
            ICollection<Team> allOfThem = teams.GetAll();
            ICollection<TeamModelOut> conversion = allOfThem.Select(t => BuildTeamModelOut(t)).ToList();
            return Ok(conversion);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            IActionResult result;
            try
            {
                Team fetched = teams.Get(id);
                TeamModelOut output = CreateModelOut(fetched);
                result = Ok(output);
            }
            catch (TeamNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = new NotFoundObjectResult(error);
            }
            return result;
        }

        [HttpGet("{sportName}/{teamName}", Name = "GetTeamById")]
        public IActionResult Get(string sportName, string teamName)
        {
            IActionResult result;
            try
            {
                Team fetched = teams.Get(sportName, teamName);
                TeamModelOut transferObject = CreateModelOut(fetched);
                result = Ok(transferObject);
            }
            catch (TeamNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = new NotFoundObjectResult(error);
            }
            return result;
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


        [HttpPost]
        public IActionResult Post([FromBody] TeamModelIn team)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = AddValidTeam(team);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult AddValidTeam(TeamModelIn team)
        {
            IActionResult result;
            try
            {
                result = TryAddTeam(team);
            }
            catch (TeamAlreadyExistsException e) {
                ErrorModelOut error = CreateErrorModel(e);
                result = BadRequest(error);
            }
            return result;
        }

        private ErrorModelOut CreateErrorModel(TeamAlreadyExistsException e)
        {
            return new ErrorModelOut() { ErrorMessage = e.Message };
        }

        private IActionResult TryAddTeam(TeamModelIn team)
        {
            IActionResult result;
            Team toAdd = new Team(team.Name, team.Photo, new Sport(team.SportName));
            Team added = teams.Add(toAdd);
            TeamModelOut modelOut = BuildTeamModelOut(added);
            return CreatedAtRoute("GetTeamById", modelOut);
        }

        private TeamModelOut BuildTeamModelOut(Team toReturn)
        {
            TeamModelOut output= new TeamModelOut()
            {
                Id = toReturn.Id,
                Name = toReturn.Name,
                Photo = toReturn.Photo
            };
            return output;
        }

        [HttpPut("{id}")]
        public IActionResult Put(string sportName, [FromBody] TeamModelIn value)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = PutValid(sportName, value);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult PutValid(string sportName, TeamModelIn value)
        {
            IActionResult result;
            try
            {
                Team toModify = new Team(1,value.Name, value.Photo,new Sport("Soccer"));
                teams.Modify(toModify);
                result = Ok();
            }
            catch (TeamNotFoundException)
            {
                result = PostId(sportName, value);
            }
            return result;
        }

        private IActionResult PostId(string sportName, TeamModelIn team)
        {
            Team toAdd = new Team(1,team.Name, team.Photo,new Sport(team.SportName));

            teams.Add(toAdd);

            TeamModelOut addedTeam = new TeamModelOut()
            {
                SportName = sportName,
                Name = team.Name,
                Photo = team.Photo
            };

            return CreatedAtRoute("GetTeamById", new { id = addedTeam.Id }, addedTeam);
        }

        [HttpDelete("{sportName}/{teamName}")]
        public IActionResult Delete(string sportName, string teamName)
        {
            IActionResult result;
            try
            {
                teams.Delete(sportName, teamName);
                result = Ok();
            }
            catch (TeamNotFoundException e)
            {
                result = BadRequest(e.Message);
            }
            return result;
        }
    }
}