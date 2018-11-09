using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.WebAPI.Models;
using System.Net;
using Microsoft.AspNetCore.Cors;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService loginService;
        private ILoggerService logger;

        public AuthenticationController(IAuthenticationService aService, ILoggerService aLogger)
        {
            loginService = aService;
            logger = aLogger;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody]LoginModelIn user)
        {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = AuthenticateWithValidModel(user);
            }
            else {
                result = BadRequest(ModelState);
                logger.Log(LogType.LOGIN, LogMessage.LOGIN_BAD_MODEL_IN, user.Username, DateTime.Now);
            }
            return result;
        }

        private IActionResult AuthenticateWithValidModel(LoginModelIn user)
        {
            IActionResult result;
            try
            {
                UserDto logged = loginService.Login(user.Username, user.Password);
                string tokenString = GenerateJSONWebToken(logged);
                result = Ok(new { Token = tokenString });
                logger.Log(LogType.LOGIN, LogMessage.LOGIN_OK, user.Username, DateTime.Now);
            }
            catch (WrongPasswordException e2)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e2.Message };
                result = BadRequest(error);
                logger.Log(LogType.LOGIN, LogMessage.LOGIN_WRONG_PASSWORD, user.Username, DateTime.Now);
            }
            catch (ServiceException e)
            {
                result = GenerateResponse(e);
                LogError(e, user.Username);
            }

            return result;
        }

        private IActionResult GenerateResponse(ServiceException e)
        {
            IActionResult errorResult;
            ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
            if (e.Error.Equals(ErrorType.ENTITY_NOT_FOUND))
            {
                errorResult = BadRequest(error);
            }
            else
            {
                errorResult = StatusCode((int)HttpStatusCode.InternalServerError, error);
            }
            return errorResult;
        }

        private void LogError(ServiceException e, string username)
        {
            //if there is data access, log
            if (e.Error.Equals(ErrorType.ENTITY_NOT_FOUND))
            {
                logger.Log(LogType.LOGIN, LogMessage.LOGIN_USER_NOT_FOUND, username, DateTime.Now);
            }
        }

        private string GenerateJSONWebToken(UserDto userInfo)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "http://localhost:5000",
               audience: "http://localhost:5000",
               claims: new List<Claim>{
                        new Claim(ClaimTypes.Role, AdminOrFollower(userInfo)),
                        new Claim(AuthenticationConstants.USERNAME_CLAIM, userInfo.username),
                        },
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string AdminOrFollower(UserDto aUser)
        {
            return aUser.isAdmin ? AuthenticationConstants.ADMIN_ROLE : AuthenticationConstants.FOLLOWER_ROLE;
        }
    }
}

