using System;
using System.Collections.Generic;
using System.Linq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.Services.Interfaces;
using System.Net;
using Microsoft.Extensions.Options;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private ISportRepository sports;
        private ITeamRepository teams;
        private IFixtureService fixture;
        private ISportTableService tableService;

        public SportsController(ISportRepository sportRepo, ITeamRepository teamRepo, 
            IFixtureService fixtureService,ISportTableService tableGenerator)
        {
            sports = sportRepo;
            fixture = fixtureService;
            teams = teamRepo;
            tableService = tableGenerator;
        }

        [HttpPost]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Post([FromBody]SportModelIn modelIn)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = CreateValidSport(modelIn);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult CreateValidSport(SportModelIn modelIn)
        {
            IActionResult result;
            try {
                result = TryAddSport(modelIn);
            }
            catch (SportAlreadyExistsException e) {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = BadRequest(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult TryAddSport(SportModelIn modelIn)
        {
            Sport toAdd = new Sport(modelIn.Name,modelIn.IsTwoTeams);
            sports.Add(toAdd);
            SportModelOut modelOut = new SportModelOut(){Name = toAdd.Name};
            IActionResult result = CreatedAtRoute("GetSportById",new {name = toAdd.Name },modelOut);
            return result;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            IActionResult result;
            try
            {
                result = TryGetAll();
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult TryGetAll()
        {
            ICollection<Sport> allOfThem = sports.GetAll();
            IEnumerable<SportModelOut> output = allOfThem.Select(s => new SportModelOut { Name = s.Name });
            return Ok(output);
        }

        [HttpGet("{name}", Name = "GetSportById")]
        [Authorize]
        public IActionResult Get(string name)
        {

            IActionResult result;
            try
            {
                result = TryGet(name);
            }
            catch (SportNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;

        }

        private IActionResult TryGet(string name)
        {
            Sport retrieved = sports.Get(name);
            SportModelOut output = new SportModelOut() { Name = retrieved.Name };
            return Ok(output);
        }

        [HttpDelete("{name}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Delete(string name)
        {
            IActionResult result;
            try
            {
                result = TryDelete(name);
            }
            catch (SportNotFoundException e)
            {
                result = NotFound(e.Message);
            }
            catch (DataInaccessibleException e) {
                result = NoDataAccess(e);
            }
            return result;
        }

        private IActionResult TryDelete(string name)
        {
            sports.Delete(name);
            OkModelOut okMessage = new OkModelOut() { OkMessage = "Sport was deleted" };
            return Ok(okMessage);
        }

        [HttpPut("{name}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Put(string name, [FromBody] SportModelIn modelIn)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = ModifyOrAdd(name, modelIn);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult ModifyOrAdd(string name, SportModelIn modelIn)
        {
            IActionResult result;
            Sport toAdd = new Sport(modelIn.Name,modelIn.IsTwoTeams);
            try
            {
                sports.Modify(toAdd);
                SportModelOut modelOut = new SportModelOut()
                {
                    Name = modelIn.Name
                };
                result = Ok(modelOut);
            }
            catch (SportNotFoundException)
            {
                sports.Add(toAdd);
                SportModelOut modelOut = new SportModelOut() { Name = toAdd.Name };
                result = CreatedAtRoute("GetSportById",new { name= toAdd.Name} ,modelOut);
            }
            catch (DataInaccessibleException e)
            {
                result = NoDataAccess(e);
            }
            return result;
        }


        [HttpGet("{name}/teams")]
        [Authorize]
        public IActionResult GetTeams(string name)
        {
            IActionResult result;
            try { 
                ICollection<Team> sportTeams = teams.GetTeams(name);
                ICollection<TeamModelOut> output = sportTeams.Select(t => CreateModelOut(t)).ToList();
                result = Ok(output);
            }
            catch (SportNotFoundException e) {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
            }
            catch (DataInaccessibleException e)
            {
                result = NoDataAccess(e);
            }
            return result;
        }

        [HttpGet("{sportName}/table")]
        [Authorize]
        public IActionResult CalculateSportTable(string sportName)
        {
            IActionResult result;
            try
            {
                result = TryCalculateTable(sportName);

            }
            catch (EntityNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
            }
            return result;
        }

        private IActionResult TryCalculateTable(string sportName)
        {
            ICollection<Tuple<Team, int>> standings = tableService.GetScoreTable(sportName);
            IEnumerable<StandingModelOut> output = standings.Select(s => CreateStanding(s));
            return Ok(output);
        }

        private StandingModelOut CreateStanding(Tuple<Team, int> standing)
        {
            Team team = standing.Item1;
            return new StandingModelOut() { TeamId = team.Id, TeamName = team.Name, Points = standing.Item2 };
        }

        private TeamModelOut CreateModelOut(Team aTeam)
        {
            return new TeamModelOut()
            {
                Id = aTeam.Id,
                SportName = aTeam.Sport.Name,
                Name = aTeam.Name,
                Photo = new byte[0]
            };
        }

        private IActionResult NoDataAccess(DataInaccessibleException e)
        {
            ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
            IActionResult internalError = StatusCode((int)HttpStatusCode.InternalServerError, error);
            return internalError;
        }
    }
}