using System;
using System.Collections.Generic;
using System.Linq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.WebAPI.Models;
using System.Net;

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
        [Authorize]
        public IActionResult Get()
        {
            IActionResult result;
            try
            {
                result = TryGet();
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult TryGet()
        {
            ICollection<Team> allOfThem = teams.GetAll();
            ICollection<TeamModelOut> conversion = allOfThem.Select(t => BuildTeamModelOut(t)).ToList();
            return Ok(conversion);
        }

        [HttpGet("{id}", Name = "GetTeamById")]
        [Authorize]
        public IActionResult Get(int id)
        {
            IActionResult result;
            try
            {
                Team fetched = teams.Get(id);
                TeamModelOut output = BuildTeamModelOut(fetched);
                result = Ok(output);
            }
            catch (TeamNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = new NotFoundObjectResult(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        [HttpGet("{sportName}/{teamName}")]
        [Authorize]
        public IActionResult Get(string sportName, string teamName)
        {
            IActionResult result;
            try
            {
                Team fetched = teams.Get(sportName, teamName);
                TeamModelOut transferObject = BuildTeamModelOut(fetched);
                result = Ok(transferObject);
            }
            catch (TeamNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = new NotFoundObjectResult(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }


        [HttpPost]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
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
            catch (TeamAlreadyExistsException e)
            {
                ErrorModelOut error = CreateErrorModel(e);
                result = BadRequest(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult TryAddTeam(TeamModelIn team)
        {
            Team toAdd = new Team(team.Name, team.Photo, new Sport(team.SportName));
            Team added = teams.Add(toAdd);
            TeamModelOut modelOut = BuildTeamModelOut(added);
            return CreatedAtRoute("GetTeamById",new {id =added.Id } ,modelOut);
        }


        [HttpPut("{teamId}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Put(int teamId, [FromBody] TeamModelIn value)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = PutValid(teamId, value);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult PutValid(int teamId, TeamModelIn value)
        {
            IActionResult result;
            try
            {
                Team toModify = new Team(teamId, value.Name, value.Photo, new Sport(value.SportName));
                teams.Modify(toModify);
                TeamModelOut output = BuildTeamModelOut(toModify);
                result = Ok(output);
            }
            catch (TeamNotFoundException)
            {
                result = PostId(teamId, value);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult PostId(int teamId, TeamModelIn team)
        {
            Team toAdd = new Team(teamId,team.Name, team.Photo,new Sport(team.SportName));
            teams.Add(toAdd);
            TeamModelOut addedTeam = BuildTeamModelOut(toAdd);
            return CreatedAtRoute("GetTeamById", new { id = addedTeam.Id }, addedTeam);
        }

        [HttpDelete("{sportName}/{teamName}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Delete(string sportName, string teamName)
        {
            IActionResult result;
            try
            {
                teams.Delete(sportName, teamName);
                OkModelOut message = new OkModelOut() { OkMessage = "The team was deleted successfully" };
                result = Ok(message);
            }
            catch (TeamNotFoundException e)
            {
                result = BadRequest(e.Message);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Delete(int id)
        {
            IActionResult result;
            try
            {
                teams.Delete(id);
                OkModelOut message = new OkModelOut { OkMessage = "The team was deleted succesfully" };
                result = Ok(message);
            }
            catch (TeamNotFoundException e)
            {
                ErrorModelOut error = CreateErrorModel(e);
                result = NotFound(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private TeamModelOut BuildTeamModelOut(Team toReturn)
        {
            TeamModelOut output = new TeamModelOut()
            {
                Id = toReturn.Id,
                SportName = toReturn.Sport.Name,
                Name = toReturn.Name,
                Photo = toReturn.Photo
            };
            return output;
        }
        private IActionResult NoDataAccess(DataInaccessibleException e)
        {
            ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
            IActionResult internalError = StatusCode((int)HttpStatusCode.InternalServerError, error);
            return internalError;
        }

        private ErrorModelOut CreateErrorModel(Exception e)
        {
            return new ErrorModelOut() { ErrorMessage = e.Message };
        }
    }
}