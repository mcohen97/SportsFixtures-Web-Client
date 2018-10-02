using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.WebAPI.Models;
using RepositoryInterface;
using BusinessLogic;
using BusinessLogic.Factories;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Exceptions;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private IMatchService matchService;

        public MatchesController(IMatchService aService)
        {
            matchService = aService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            ICollection<Match> matches = matchService.GetAllMatches();
            ICollection<MatchModelOut> output = matches.Select(m => BuildModelOut(m)).ToList();
            return Ok(output);
        }

        [HttpPost]
        public IActionResult Post([FromBody] MatchModelIn input)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = TryPostMatch(input);
            }
            else {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult TryPostMatch(MatchModelIn input)
        {
            IActionResult result;
            try
            {
                Match added = matchService.AddMatch(input.HomeTeamId, input.AwayTeamId, input.SportName, input.Date);
                MatchModelOut output =BuildModelOut(added);
                result = CreatedAtRoute("GetMatchById", output);
            }
            catch (EntityNotFoundException e) {
                result = CreateErrorMessage(e);
            }
            catch (TeamAlreadyHasMatchException e) {
                result = CreateErrorMessage(e);
            }
            return result;
        }

        private MatchModelOut BuildModelOut(Match aMatch)
        {
           return new MatchModelOut()
            {
                Id =aMatch.Id,
                SportName = aMatch.Sport.Name,
                AwayTeamId = aMatch.AwayTeam.Id,
                HomeTeamId = aMatch.HomeTeam.Id,
                CommentsIds = aMatch.GetAllCommentaries().Select(c => c.Id).ToList()
            };
        }

        [HttpGet("{matchId}")]
        public IActionResult Get(int matchId)
        {
            IActionResult result;
            try
            {
                result = TryGetMatch(matchId);
            }
            catch(MatchNotFoundException e) {
                result = CreateErrorMessage(e);
            }
            return result;
        }

        private IActionResult TryGetMatch(int matchId)
        {
            Match stored = matchService.GetMatch(matchId);
            MatchModelOut modelOut = BuildModelOut(stored);
            IActionResult result = Ok(modelOut);
            return result;
        }

        private IActionResult CreateErrorMessage(Exception e)
        {
            ErrorModelOut error = new ErrorModelOut { ErrorMessage = e.Message };
            IActionResult errorResult = BadRequest(error);
            return errorResult;
        }
    }
}
