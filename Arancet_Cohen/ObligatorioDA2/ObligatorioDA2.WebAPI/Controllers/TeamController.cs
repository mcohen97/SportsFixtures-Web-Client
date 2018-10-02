using System;
using System.Collections.Generic;
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
            return Ok();
        }


        [HttpGet("{sportName}/{teamName}", Name = "GetTeamById")]
        public IActionResult Get(string sportName, string teamName)
        {
            IActionResult result;
            Team fetched;
            try
            {
                fetched = TryGetTeam(sportName, teamName);
                TeamModelOut transferObject = CreateModelOut(fetched);
                result = Ok(transferObject);
            }
            catch (TeamNotFoundException e)
            {
                result = new NotFoundObjectResult(e.Message);
            }
            return result;
        }

        private Team TryGetTeam(string sportName, string teamName)
        {
            return teams.Get(sportName, teamName);
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
            IActionResult toReturn;
            if (ModelState.IsValid)
            {
                Team toAdd = new Team(team.Name, team.Photo, new Sport(team.SportName));

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