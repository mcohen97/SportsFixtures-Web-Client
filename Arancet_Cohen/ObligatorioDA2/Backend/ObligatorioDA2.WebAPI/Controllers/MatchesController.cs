using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private IMatchService matchService;
        private EncounterModelFactory factory;
        private ErrorActionResultFactory errors;

        public MatchesController(IMatchService aService)
        {
            matchService = aService;
            factory = new EncounterModelFactory();
            errors = new ErrorActionResultFactory(this);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            ICollection<EncounterDto> matches = matchService.GetAllMatches();
            ICollection<EncounterModelOut> output = matches.Select(m => factory.CreateModelOut(m)).ToList();
            return Ok(output);
        }

        [HttpPost]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
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
                EncounterDto added = matchService.AddMatch(input.TeamIds, input.SportName, input.Date);
                EncounterModelOut output = factory.CreateModelOut(added);
                result = CreatedAtRoute("GetMatchById",new {matchId = added.id }, output);
            }
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
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
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryGetMatch(int matchId)
        {
            EncounterDto stored = matchService.GetMatch(matchId);
            EncounterModelOut modelOut = factory.CreateModelOut(stored);
            IActionResult result = Ok(modelOut);
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
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
            try
            {
                matchService.ModifyMatch(id, aMatch.TeamIds, aMatch.Date, aMatch.SportName);
                EncounterModelOut output = BuildModelout(id, aMatch);
                result = Ok(output);
            }
            catch (ServiceException e)
            {
                if (e.Error.Equals(ErrorType.ENTITY_NOT_FOUND))
                {
                    EncounterDto added = matchService.AddMatch(id, aMatch.TeamIds, aMatch.SportName, aMatch.Date);
                    EncounterModelOut output = factory.CreateModelOut(added);
                    result = CreatedAtRoute("GetMatchById", new { matchId = added.id }, output);
                }
                else {
                    result = errors.GenerateError(e);
                }
            }
            return result;
        }

        private EncounterModelOut BuildModelout(int id,MatchModelIn aMatch)
        {

            EncounterModelOut output = new MatchModelOut()
            {
                Id = id,
                SportName = aMatch.SportName,
                TeamsIds = aMatch.TeamIds,
                HasResult = false,
                Date = aMatch.Date
            };
            return output;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Delete(int id)
        {
            IActionResult result;
            try {
                result = TryToDelete(id);
            }
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryToDelete(int id)
        {
            matchService.DeleteMatch(id);
            OkModelOut okMessage = new OkModelOut() { OkMessage = "The match was deleted successfully" };
            return Ok(okMessage);
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
                result = TryAddComment(matchId, input);
            }
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryAddComment(int matchId,CommentModelIn input)
        {
            string username = HttpContext.User.Claims.First(c => c.Type.Equals(AuthenticationConstants.USERNAME_CLAIM)).Value;
            CommentaryDto created = matchService.CommentOnMatch(matchId, username, input.Text);
            CommentModelOut output = new CommentModelOut
            {
                Id = created.commentId,
                MakerUsername = username,
                Text = input.Text
            };
            return CreatedAtRoute("GetCommentById",new {id =output.Id }, output);
        }

        [HttpGet("sport/{sportName}")]
        [Authorize]
        public IActionResult GetBySport(string sportName)
        {
            IActionResult result;
            try {
                ICollection<EncounterDto> matches = matchService.GetAllEncounterDtos(sportName);
                ICollection<EncounterModelOut> output = matches.Select(m => factory.CreateModelOut(m)).ToList();
                result = Ok(output);
            }
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }
            return result;
        }

        [HttpGet("team/{teamId}")]
        public IActionResult GetByTeam(int teamId)
        {
            IActionResult result;
            try
            {
                ICollection<EncounterDto> matches = matchService.GetAllEncounterDtos(teamId);
                ICollection<EncounterModelOut> output = matches.Select(m => factory.CreateModelOut(m)).ToList();
                result = Ok(output);
            }
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }


        [HttpGet("{matchId}/comments", Name = "GetCommentMatchComments")]
        public IActionResult GetMatchComments(int matchId) {
            IActionResult result;
            try
            {
                result = TryGetMatchComments(matchId);
            }
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryGetMatchComments(int matchId)
        {
            ICollection<CommentaryDto> matchComments = matchService.GetMatchCommentaries(matchId);
            ICollection<CommentModelOut> output = matchComments.Select(c => BuildCommentModelOut(c)).ToList();
            return Ok(output);
        }

        [HttpGet("comments")]
        public IActionResult GetAllComments()
        {
            IActionResult result;
            try
            {
                result = TryGetAllComments();
            }
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryGetAllComments()
        {
            ICollection<CommentaryDto> allComments = matchService.GetAllCommentaries();
            ICollection<CommentModelOut> output = allComments.Select(c => BuildCommentModelOut(c)).ToList();
            return Ok(output);
        }

        private CommentModelOut BuildCommentModelOut(CommentaryDto aComment) {
            CommentModelOut comment = new CommentModelOut()
            {
                Id = aComment.commentId,
                MakerUsername = aComment.makerUsername,
                Text = aComment.text
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
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryGetComment(int id)
        {
            CommentaryDto comment = matchService.GetComment(id);
            CommentModelOut output = new CommentModelOut
            {
                Id = comment.commentId,
                MakerUsername = comment.makerUsername,
                Text = comment.text
            };
            return Ok(output);
        }

        [HttpPost("{matchId}/result")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult SetResult(int matchId, ResultModel resultModel)
        {
            IActionResult result;
            try {
                result = TrySetResult(matchId, resultModel);
            }
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TrySetResult(int matchId, ResultModel resultModel)
        {
            ICollection<Tuple<int, int>> team_positions = resultModel.Team_Position
                .Select(tp => new Tuple<int, int>(tp.TeamId, tp.Position)).ToList();
            ResultDto encounterResult = new ResultDto() { teams_positions = team_positions };
            matchService.SetResult(matchId, encounterResult);
            EncounterDto matchWithResult = matchService.GetMatch(matchId);
            EncounterModelOut result = factory.CreateModelOut(matchWithResult);
            return Ok(result);
        }
    }
}
