using System;
using System.Collections.Generic;
using BusinessLogic;
using DataRepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.WebAPI.Models;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private ISportRepository sports;
        public SportsController(ISportRepository aRepo) {
            sports=aRepo;
        }

        [HttpPost]
        public IActionResult Post([FromBody]SportModelIn modelIn)
        {
            Sport toAdd = new Sport(modelIn.Name);
            sports.Add(toAdd);
            SportModelOut modelOut = new SportModelOut()
            {
                Id = 1,
                Name = toAdd.Name
            };
            IActionResult result = CreatedAtRoute("GetById", new { id = modelOut.Id }, modelOut);
            return result;
        }

    }
}