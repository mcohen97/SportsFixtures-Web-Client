using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.WebAPI.Models;
using System.Net;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private ILoginService loginService;
        private ILoggerService logger;

        public AuthenticationController(ILoginService aService, ILoggerService aLogger)
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
            }
            return result;
        }

        private IActionResult AuthenticateWithValidModel(LoginModelIn user)
        {
            IActionResult result;
            try
            {
                User logged = loginService.Login(user.Username, user.Password);
                string tokenString = GenerateJSONWebToken(logged);
                result = Ok(new { Token = tokenString });
                logger.Log(LogType.LOGIN, LogMessage.LOGIN_OK, user.Username, DateTime.Now);
            }
            catch (UserNotFoundException e1)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e1.Message };
                result = BadRequest(error);
                logger.Log(LogType.LOGIN, LogMessage.LOGIN_USER_NOT_FOUND, user.Username, DateTime.Now);
            }
            catch (WrongPasswordException e2)
            {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e2.Message };
                result = BadRequest(error);
                logger.Log(LogType.LOGIN, LogMessage.LOGIN_WRONG_PASSWORD, user.Username, DateTime.Now);
            }
            catch (DataInaccessibleException e) {
                ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
                result = StatusCode((int)HttpStatusCode.InternalServerError, error);
                logger.Log(LogType.LOGIN, LogMessage.LOGIN_DATAINACCESSIBLE, user.Username, DateTime.Now);
            }
            return result;
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "http://localhost:5000",
               audience: "http://localhost:5000",
               claims: new List<Claim>{
                        new Claim(ClaimTypes.Role, AdminOrFollower(userInfo)),
                        new Claim(AuthenticationConstants.USERNAME_CLAIM, userInfo.UserName),
                        },
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string AdminOrFollower(User aUser)
        {
            return aUser.IsAdmin ? AuthenticationConstants.ADMIN_ROLE : AuthenticationConstants.FOLLOWER_ROLE;
        }
    }
}

