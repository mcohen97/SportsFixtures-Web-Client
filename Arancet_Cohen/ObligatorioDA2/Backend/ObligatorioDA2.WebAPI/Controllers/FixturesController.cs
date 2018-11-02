using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.Services.Interfaces;
using Microsoft.Extensions.Options;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using System.Net;
using System.Reflection;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixturesController : ControllerBase
    {
        private IFixtureService fixtureService;
        private IOptions<FixtureStrategies> fixtureConfig;
        private ISportRepository sports;
        private const string DLL_EXTENSION = "*.dll";
        private ILoggerService logger;
        private EncounterModelFactory factory;

        public FixturesController(IFixtureService service, IOptions<FixtureStrategies> config, ISportRepository sportsRepo, ILoggerService loggerService) {
            fixtureService = service;
            fixtureConfig = config;
            sports = sportsRepo;
            logger = loggerService;
            factory = new EncounterModelFactory();
        }

        [HttpGet]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult GetFixtureAlgorithms()
        {
            string algorithmsPath = fixtureConfig.Value.DllPath;        
            ICollection<Type> algorithms = fixtureService.GetAlgorithms(algorithmsPath);
            ICollection<string> toReturn = algorithms.Select(t => t.ToString()).ToList();
            return Ok(toReturn);
        }

        private string GetTypeString(Type t)
        {
            string fullName = t.ToString();
            string[] tokens = fullName.Split(".");
            return tokens[tokens.Length - 1];
        }

        [HttpPost("{sportName}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult CreateFixture(string sportName, FixtureModelIn input)
        {
            IActionResult result;
            string username = GetUserPerformingAction();

            if (ModelState.IsValid)
            {
                result = TryCreateFixture(sportName, input, username);
            }
            else
            {
                result = BadRequest(ModelState);
                logger.Log(LogType.FIXTURE, LogMessage.FIXTURE_BAD_MODEL_IN, username, DateTime.Now);
            }
            return result;
        }

        private string GetUserPerformingAction()
        {
            string username = "";
            if (HttpContext != null && HttpContext.User.Identity is ClaimsIdentity identity)
            {
                IEnumerable<Claim> claims = identity.Claims;
                username = identity.FindFirst(AuthenticationConstants.USERNAME_CLAIM).Value;
            }
            else
            {
                username = LogMessage.UNIDENTIFIED;
            }

            return username;
        }

        private IActionResult TryCreateFixture(string sportName, FixtureModelIn input, string username)
        {
            IActionResult result;
            if (ValidDate(input))
            {
                result = CreateValid(input, sportName, username);
            }
            else
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = "Invalid date format" };
                result = BadRequest(error);
                logger.Log(LogType.FIXTURE, LogMessage.FIXTURE_BAD_MODEL_IN, username, DateTime.Now);
            }
            return result;
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

        private IActionResult CreateValid(FixtureModelIn input, string sportName, string username)
        {
            IActionResult result;
            try
            {
                fixtureService.FixtureAlgorithm = BuildFixtureAlgorithm(new DateTime(input.Year, input.Month, input.Day), input.FixtureName);
                result = TryCreate(input, sportName);
                logger.Log(LogType.FIXTURE, LogMessage.FIXTURE_OK, username, DateTime.Now);
            }
            catch (WrongFixtureException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = BadRequest(error);
                logger.Log(LogType.FIXTURE, LogMessage.FIXTURE_WRONG + " " + e.Message, username, DateTime.Now);
            }
            catch (EntityNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
                logger.Log(LogType.FIXTURE, LogMessage.FIXTURE_SPORT_NOT_FOUND, username, DateTime.Now);
            }
            catch (DataInaccessibleException e)
            {
                result = NoDataAccess(e);
                logger.Log(LogType.FIXTURE, LogMessage.FIXTURE_DATAINACCESSIBLE, username, DateTime.Now);
            }

            return result;
        }
        private IFixtureGenerator BuildFixtureAlgorithm(DateTime date, string fixtureName)
        {
            string algorithmsPath = fixtureConfig.Value.DllPath;
            Type algortihmType = GetAlgorithmType(algorithmsPath, fixtureName);
            object fromDll = Activator.CreateInstance(algortihmType, new object[] { date, 1, 7 });
            IFixtureGenerator algorithm = fromDll as IFixtureGenerator;
            return algorithm;
        }

        private Type GetAlgorithmType(string algorithmsPath, string fixtureName)
        {
            bool found = false;
            string[] files = Directory.GetFiles(algorithmsPath, DLL_EXTENSION);
            Type first2comply = null;

            for (int i=0; i< files.Length && !found; i++) {
                Assembly actual =Assembly.LoadFrom(files[i]);
                first2comply = actual.GetType(fixtureName);
                if (first2comply != null) {
                    found = true;
                }
            }

            if (first2comply == null) {
                throw new WrongFixtureException("Fixture not found");
            }
            return first2comply;
        }

        private IActionResult TryCreate(FixtureModelIn input, string sportName)
        {
            IActionResult result;
            Sport sport = sports.Get(sportName);
            ICollection<Encounter> added = fixtureService.AddFixture(sport);
            ICollection<EncounterModelOut> addedModelOut = new List<EncounterModelOut>();
            foreach (Encounter match in added)
            {
                addedModelOut.Add(factory.CreateModelOut(match));
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
