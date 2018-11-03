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
using System.Text;
using System.IO;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private ITeamService teamService;
        private IImageService images;
        IAuthenticationService authentication;
        private ErrorActionResultFactory errors;
        private const string IMG_EXTENSION = ".jpg";

        public TeamsController(ITeamService aService, IImageService imageManager, IAuthenticationService authService)
        {
            teamService = aService;
            errors = new ErrorActionResultFactory(this);
            images = imageManager;
            authentication = authService;

        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            SetSession();
            IActionResult result;
            try
            {
                result = TryGet();
            }
            catch (ServiceException e) {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private IActionResult TryGet()
        {
            ICollection<Team> allOfThem = teamService.GetAllTeams();
            ICollection<TeamModelOut> conversion = allOfThem.Select(t => BuildTeamModelOut(t)).ToList();
            return Ok(conversion);
        }

        [HttpGet("{id}", Name = "GetTeamById")]
        [Authorize]
        public IActionResult Get(int id)
        {
            SetSession();
            IActionResult result;
            try
            {
                Team fetched = teamService.GetTeam(id);
                TeamModelOut output = BuildTeamModelOut(fetched);
                result = Ok(output);
            }
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }

            return result;
        }

        [HttpPost]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Post([FromBody] TeamModelIn team)
        {
            SetSession();
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
            SetSession();
            IActionResult result;
            try
            {

                result = TryAddTeam(team);
            }
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }
            return result;
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private IActionResult TryAddTeam(TeamModelIn team)
        {
            string imgData = Base64Encode(team.Photo);
            team.Photo = team.Name + "_" + team.SportName + IMG_EXTENSION;
            TeamDto dto = BuildTransferObject(team);
            Team added =teamService.AddTeam(dto);
            TeamModelOut modelOut = BuildTeamModelOut(added);
            images.SaveImage(team.Photo, imgData);
            return CreatedAtRoute("GetTeamById",new {id =added.Id } ,modelOut);

        }

        private TeamDto BuildTransferObject(TeamModelIn team)
        {
            return new TeamDto() { id = team.Id, name = team.Name, photo = team.Photo, sportName = team.SportName };
        }

        [HttpPut("{teamId}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Put(int teamId, [FromBody] TeamModelIn value)
        {
            SetSession();
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = TryPut(teamId, value);
            }
            else
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult TryPut(int teamId, TeamModelIn value)
        {
            IActionResult result;
            try
            {
                value.Id = teamId;
                TeamDto dto = BuildTransferObject(value);
                Team modified = teamService.Modify(dto);
                TeamModelOut output = BuildTeamModelOut(modified);
                result = Ok(output);
            }
            catch (ServiceException e)
            {
                if (e.Error.Equals(ErrorType.ENTITY_NOT_FOUND))
                {
                    result = Post(value);
                }
                else {
                    result = errors.GenerateError(e);
                }
            }

            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult Delete(int id)
        {
            SetSession();
            IActionResult result;
            try
            {
                teamService.DeleteTeam(id);
                OkModelOut message = new OkModelOut { OkMessage = "The team was deleted succesfully" };
                result = Ok(message);
            }
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
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
                Photo = images.ReadImage(toReturn.PhotoPath)
            };
            return output;
        }

        private void SetSession() {
            string username = HttpContext.User.Claims.First(c => c.Type.Equals(AuthenticationConstants.USERNAME_CLAIM)).Value;
            authentication.SetSession(username);
        }
    }
}