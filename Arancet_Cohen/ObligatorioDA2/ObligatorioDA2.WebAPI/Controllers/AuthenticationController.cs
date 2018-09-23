using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.Services;
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

        // POST api/<controller>
        [HttpPost, Route("login")]
        public IActionResult Authenticate([FromBody]LoginModelIn user)
        {
            User logged = logger.Login(user.Username, user.Password);
            return Ok(logged);
        }
    }
}

