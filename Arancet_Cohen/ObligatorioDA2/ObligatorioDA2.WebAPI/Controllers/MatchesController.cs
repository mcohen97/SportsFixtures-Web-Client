using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.WebAPI.Models;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Authorize]
        public IActionResult Get()
        {
            ICollection<Match> matches = matchService.GetAllMatches();
            ICollection<MatchModelOut> output = matches.Select(m => BuildModelOut(m)).ToList();
            return Ok(output);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                MatchModelOut output = BuildModelOut(added);
                result = CreatedAtRoute("GetMatchById",new {matchId = added.Id }, output);
            }
            catch (DataAccessException e) {
                result = CreateErrorMessage(e);
            }
            catch (TeamAlreadyHasMatchException e) {
                result = CreateErrorMessage(e);
            }
            return result;
        }


        [HttpGet("{matchId}", Name = "GetMatchById")]
        [Authorize]
        public IActionResult Get(int matchId)
        {
            IActionResult result;
            try
            {
                result = TryGetMatch(matchId);
            }
            catch (MatchNotFoundException e) {
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

        [HttpPut("{id}")]
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
            }
            catch (TeamAlreadyHasMatchException e) {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = BadRequest(error);
            }catch (DataAccessException e) {
                Match added = matchService.AddMatch(id, aMatch.HomeTeamId,
                     aMatch.AwayTeamId, aMatch.SportName, aMatch.Date);
                MatchModelOut output = BuildModelOut(added);
                result = CreatedAtRoute("GetMatchById",new {matchId =added.Id } ,output);
            }
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            IActionResult result;
            try {
                result = TryToDelete(id);
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

        [HttpPost("{matchId}/comments")]
        [Authorize]
        public IActionResult CommentOnMatch(int matchId,CommentModelIn input)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = AddValidFormatComment(matchId,input);
            }
            else {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult AddValidFormatComment(int matchId,CommentModelIn input)
        {
            IActionResult result;
            try
            {
                result = TryAddComment(matchId,input);
            }
            catch (DataAccessException e) {
                result = CreateErrorMessage(e);
            }
            return result;
        }

        private IActionResult TryAddComment(int matchId,CommentModelIn input)
        {
            string username = HttpContext.User.Claims.First(c => c.Type.Equals("Username")).Value;
            Commentary created = matchService.CommentOnMatch(matchId, username, input.Text);
            CommentModelOut output = new CommentModelOut
            {
                Id = created.Id,
                MakerUsername = username,
                Text = input.Text
            };
            return CreatedAtRoute("GetCommentById",new {id =created.Id }, output);
        }

        private IActionResult CreateErrorMessage(Exception e)
        {
            ErrorModelOut error = new ErrorModelOut { ErrorMessage = e.Message };
            IActionResult errorResult = BadRequest(error);
            return errorResult;
        }

        [HttpGet("sport/{sportName}")]
        [Authorize]
        public IActionResult GetBySport(string sportName)
        {
            IActionResult result;
            try {
                ICollection<Match> matches = matchService.GetAllMatches(sportName);
                ICollection<MatchModelOut> output = matches.Select(m => BuildModelOut(m)).ToList();
                result = Ok(output);
            }
            catch (SportNotFoundException e)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
            }
            return result;
        }

        [HttpGet("team/{teamId}")]
        public IActionResult GetByTeam(int teamId)
        {
            IActionResult result;
            try
            {
                ICollection<Match> matches = matchService.GetAllMatches(teamId);
                ICollection<MatchModelOut> output = matches.Select(m => BuildModelOut(m)).ToList();
                result = Ok(output);
            }
            catch (TeamNotFoundException e) {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
            }
            return result;
        }
        private MatchModelOut BuildModelOut(Match aMatch)
        {
            return new MatchModelOut()
            {
                Id = aMatch.Id,
                SportName = aMatch.Sport.Name,
                AwayTeamId = aMatch.AwayTeam.Id,
                HomeTeamId = aMatch.HomeTeam.Id,
                Date= aMatch.Date,
                CommentsIds = aMatch.GetAllCommentaries().Select(c => c.Id).ToList()
            };
        }

        [HttpGet("{matchId}/comments", Name = "GetCommentMatchComments")]
        public IActionResult GetMatchComments(int matchId) {

            ICollection<Commentary> matchComments = matchService.GetMatchCommentaries(matchId);
            ICollection<CommentModelOut> output = matchComments.Select(c => BuildCommentModelOut(c)).ToList();
            return Ok(output);
        }

        [HttpGet("comments")]
        public IActionResult GetAllComments()
        {
            ICollection<Commentary> allComments = matchService.GetAllCommentaries();
            ICollection<CommentModelOut> output = allComments.Select(c => BuildCommentModelOut(c)).ToList();
            return Ok(output);
        }

        private CommentModelOut BuildCommentModelOut(Commentary aComment) {
            CommentModelOut comment = new CommentModelOut()
            {
                Id = aComment.Id,
                MakerUsername = aComment.Maker.UserName,
                Text = aComment.Text
            };
            return comment;
        }

        [HttpGet("comments/{id}", Name = "GetCommentById")]
        public IActionResult GetComment(int id)
        {
            IActionResult result;
            try
            {
                result = TryGetComment(id);

            }
            catch (CommentNotFoundException e) {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = NotFound(error);
            }
            return result;
        }

        private IActionResult TryGetComment(int id)
        {
            Commentary comment = matchService.GetComment(id);
            CommentModelOut output = new CommentModelOut
            {
                Id = comment.Id,
                MakerUsername = comment.Maker.UserName,
                Text = comment.Text
            };
            return Ok(output);
        }
    }
}
