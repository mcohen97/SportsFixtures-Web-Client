using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.Services;
using DataRepositories;
using ObligatorioDA2.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private ISportRepository sports;
        private ITeamRepository teams;
        private IMatchRepository matches;


        public SportsController(ISportRepository sportRepo, IMatchRepository matchRepo, ITeamRepository teamRepo)
        {
            sports = sportRepo;
            matches = matchRepo;
            teams = teamRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
            return result;
        }

        private IActionResult TryAddSport(SportModelIn modelIn)
        {
            Sport toAdd = new Sport(modelIn.Name);
            sports.Add(toAdd);
            SportModelOut modelOut = new SportModelOut(){Name = toAdd.Name};
            IActionResult result = CreatedAtRoute("GetSportById",new {name = toAdd.Name },modelOut);
            return result;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
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
                result = NotFound(e.Message);
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
        [Authorize(Roles = "Admin")]
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
            return result;
        }

        private IActionResult TryDelete(string name)
        {
            sports.Delete(name);
            return Ok();
        }

        [HttpPut("{name}")]
        [Authorize(Roles = "Admin")]
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
            Sport toAdd = new Sport(modelIn.Name);
            try
            {
                sports.Modify(toAdd);
                UserModelOut modelOut = new UserModelOut()
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
            return result;
        }

        private TeamModelOut CreateModelOut(Team aTeam)
        {
            return new TeamModelOut()
            {
                Id = aTeam.Id,
                SportName = aTeam.Sport.Name,
                Name = aTeam.Name,
                Photo = aTeam.Photo
            };
        }

        [HttpPost("{sportName}/OneMatchFixture")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateOneMatchFixture(string sportName, [FromBody] FixtureModelIn input)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                IFixtureService fixture = new FixtureService(matches, teams, sports)
                {
                    FixtureAlgorithm = new OneMatchFixture(new DateTime(input.Year, input.Month, input.Day), 1, 7)
                };
                result = CreateValid(input, sportName, fixture);
            }
            else
                result = BadRequest(ModelState);

            return result;
        }

        private IActionResult CreateValid(FixtureModelIn input, string sportName, IFixtureService fixture)
        {
            IActionResult result;
            try { 
            Sport sport = sports.Get(sportName);
                ICollection<Match> added = fixture.AddFixture(sport);
                ICollection<MatchModelOut> addedModelOut = new List<MatchModelOut>();
                foreach (Match match in added)
                {
                    addedModelOut.Add(new MatchModelOut()
                    {
                        Id = match.Id,
                        AwayTeamId = match.AwayTeam.Id,
                        HomeTeamId = match.HomeTeam.Id,
                        SportName = match.Sport.Name,
                        Date = match.Date,
                        CommentsIds = match.GetAllCommentaries().Select(c => c.Id).ToList()
                    });
                }
                result = Created("fixture-generator", addedModelOut);
            }
            catch (WrongFixtureException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = BadRequest(error);
            }
            catch (TeamNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
            }

            return result;
        }

        [HttpPost("{sportName}/HomeAwayFixture")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateHomeAwayFixture(string sportName, FixtureModelIn input)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                IFixtureService fixture = new FixtureService(matches, teams, sports)
                {
                    FixtureAlgorithm = new HomeAwayFixture(new DateTime(input.Year, input.Month, input.Day), 2, 5)
                };
                result = CreateValid(input, sportName, fixture);
            }
            else
                result = BadRequest(ModelState);

            return result;
        }
    }
}