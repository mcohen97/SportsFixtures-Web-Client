using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.WebAPI.Models;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private ILoginService logger;

        public AuthenticationController(ILoginService aService)
        {
            logger = aService;
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
                User logged = logger.Login(user.Username, user.Password);
                string tokenString = GenerateJSONWebToken(logged);
                result = Ok(new { Token = tokenString });
            }
            catch (UserNotFoundException e1)
            {
                result = BadRequest(e1.Message);
            }
            catch (WrongPasswordException e2)
            {
                result = BadRequest(e2.Message);
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
                        new Claim("Username", userInfo.UserName),
                        },
               expires: DateTime.Now.AddMinutes(5),
               signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string AdminOrFollower(User aUser)
        {
            return aUser.IsAdmin ? "Admin" : "Follower";
        }
    }
}

