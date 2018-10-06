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

        [HttpGet("{matchId}", Name = "GetMatchById")]
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


        public IActionResult Put(int id, MatchModelIn aMatch)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = TryPut(id, aMatch);
            }
            else {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult TryPut(int id, MatchModelIn aMatch)
        {
            IActionResult result;
            try {

                matchService.ModifyMatch(id, aMatch.HomeTeamId,
                     aMatch.AwayTeamId, aMatch.Date, aMatch.SportName);
                MatchModelOut output = new MatchModelOut()
                {
                    Id = id,
                    SportName = aMatch.SportName,
                    HomeTeamId = aMatch.HomeTeamId,
                    AwayTeamId = aMatch.AwayTeamId,
                    Date = aMatch.Date
                };
                result = Ok(output);
            } catch (EntityNotFoundException e) {
                Match added = matchService.AddMatch(id, aMatch.HomeTeamId,
                     aMatch.AwayTeamId, aMatch.SportName,aMatch.Date);
                MatchModelOut output = BuildModelOut(added);
                result = CreatedAtRoute("GetMatchById", output);
            }
            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            IActionResult result;
            try {
                result =TryToDelete(id);
            }
            catch (MatchNotFoundException e) {
                result = CreateErrorMessage(e);
            }
            return result;
        }

        private IActionResult TryToDelete(int id)
        {
            matchService.DeleteMatch(id);
            return Ok();
        }

        [HttpPost("comments")]
        public IActionResult CommentOnMatch(CommentModelIn input)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = AddValidFormatComment(input);
            }
            else {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult AddValidFormatComment(CommentModelIn input)
        {
            IActionResult result;
            try
            {
                result = TryAddComment(input);
            }
            catch (EntityNotFoundException e) {
                result = CreateErrorMessage(e);
            }
            return result;
        }

        private IActionResult TryAddComment(CommentModelIn input)
        {
            Commentary created = matchService.CommentOnMatch(input.MatchId, input.MakerUsername, input.Text);
            CommentModelOut output = new CommentModelOut
            {
                Id = created.Id,
                MakerUsername = input.MakerUsername,
                MatchId = input.MatchId,
                Text = input.Text
            };
            return CreatedAtRoute("GetCommentById", output);
        }

        private IActionResult CreateErrorMessage(Exception e)
        {
            ErrorModelOut error = new ErrorModelOut { ErrorMessage = e.Message };
            IActionResult errorResult = BadRequest(error);
            return errorResult;
        }

        public IActionResult GetBySport(string sportName)
        {
            throw new NotImplementedException();
        }
    }
}
