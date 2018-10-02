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

        private MatchModelOut BuildModelOut(Match aMatch)
        {

            return new MatchModelOut()
            {
                SportName = aMatch.Sport.Name,
                AwayTeamId = aMatch.AwayTeam.Id,
                HomeTeamId = aMatch.HomeTeam.Id,
                CommentsIds = aMatch.GetAllCommentaries().Select(c => c.Id).ToList()
            };
        }
    }
}
