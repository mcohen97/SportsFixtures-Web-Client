using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.Services.Interfaces;
using Microsoft.Extensions.Options;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using System.Net;
using System.Reflection;
using ObligatorioDA2.BusinessLogic.FixtureAlgorithms;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixturesController : ControllerBase
    {
        private IFixtureService fixtureService;
        private IOptions<FixtureStrategies> fixtureConfig;
        private ISportRepository sports;

        public FixturesController(IFixtureService service, IOptions<FixtureStrategies> config, ISportRepository sportsRepo) {
            fixtureService = service;
            fixtureConfig = config;
            sports = sportsRepo;
        }

        [HttpGet]
        public IActionResult GetFixtureAlgorithms()
        {
            string algorithmsPath =fixtureConfig.Value.DllPath;
            ICollection<Type> algorithms = fixtureService.GetAlgorithms(algorithmsPath);
            ICollection<string> toReturn = algorithms.Select(t => GetTypeString(t)).ToList();
            return Ok(toReturn);
        }

        private string GetTypeString(Type t)
        {
            string fullName = t.ToString();
            string[] tokens = fullName.Split(".");
            return tokens[tokens.Length - 1];
        }

        [HttpPost("{sportName}")]
        public IActionResult CreateFixture(string sportName, FixtureModelIn input) {
            IActionResult result;
            if (ModelState.IsValid)
            {
                DateTime initialDate;
                if (ValidDate(input))
                {
                    initialDate = new DateTime(input.Year, input.Month, input.Day);
                    fixtureService.FixtureAlgorithm = BuildFixtureAlgorithm(new DateTime(input.Year, input.Month, input.Day),input.FixtureName);
                    result = CreateValid(input, sportName);
                }
                else
                {
                    ErrorModelOut error = new ErrorModelOut() { ErrorMessage = "Invalid date format" };
                    result = BadRequest(error);
                }

            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IFixtureGenerator BuildFixtureAlgorithm(DateTime date,string fixtureName)
        {
            string algorithmsPath = fixtureConfig.Value.DllPath;
            Type algortihmType = GetAlgorithmType(algorithmsPath,fixtureName);
            object fromDll = Activator.CreateInstance(algortihmType, new object[] { date, 1,7});
            IFixtureGenerator algorithm = fromDll as IFixtureGenerator;
            return algorithm;
        }

        private Type GetAlgorithmType(string algorithmsPath, string fixtureName)
        {
            Assembly fixtures = Assembly.LoadFile(algorithmsPath);
            string[] pathTokens = algorithmsPath.Split("\\");
            string[] assemblyNameTokens = pathTokens[pathTokens.Length - 1].Split(".");
            assemblyNameTokens[assemblyNameTokens.Length - 1] = fixtureName;
            string typeName = String.Join(".", assemblyNameTokens);
            Type algorithmType = null;
            foreach (Type t in fixtures.GetTypes()) {
                if (t.ToString().Equals(typeName)) {
                    algorithmType = t;
                }
            }
            return algorithmType;
        }

        private bool ValidDate(FixtureModelIn input)
        {
            bool result = true;
            try
            {
                DateTime date = new DateTime(input.Year, input.Month, input.Day);
                result = date != null;
            }
            catch (ArgumentOutOfRangeException exp)
            {
                result = false;
            }
            return result;
        }

        private IActionResult CreateValid(FixtureModelIn input, string sportName)
        {
            IActionResult result;
            try
            {
                result = TryCreate(input, sportName);
            }
            catch (WrongFixtureException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = BadRequest(error);
            }
            catch (EntityNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
            }
            catch (DataInaccessibleException e)
            {
                result = NoDataAccess(e);
            }

            return result;
        }

        private IActionResult TryCreate(FixtureModelIn input, string sportName)
        {
            IActionResult result;
            Sport sport = sports.Get(sportName);
            ICollection<Match> added = fixtureService.AddFixture(sport);
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
            return result;
        }

        private IActionResult NoDataAccess(DataInaccessibleException e)
        {
            ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
            IActionResult internalError = StatusCode((int)HttpStatusCode.InternalServerError, error);
            return internalError;
        }
    }
}
