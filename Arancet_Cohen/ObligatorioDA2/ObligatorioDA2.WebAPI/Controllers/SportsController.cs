using System;
using System.Collections.Generic;
using System.Linq;
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
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = CreateValidSport(modelIn);
            }
            else {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult CreateValidSport(SportModelIn modelIn) {
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

        [HttpGet]
        public IActionResult Get() {
            ICollection<Sport> allOfThem = sports.GetAll();
            IEnumerable<SportModelOut> output = allOfThem.Select(s=> new SportModelOut {Name=s.Name, Id=s.Id });
            return Ok(output);
        }

        

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult Get(int id)
        {

            IActionResult result;
            try
            {
                result = TryGet(id);
            }
            catch (SportNotFoundException e) {
                result = NotFound(e.Message);
            }
            return result;
            
        }

        private IActionResult TryGet(int id)
        {
            Sport retrieved = sports.Get(id);
            SportModelOut output = new SportModelOut() { Id = id, Name = retrieved.Name };
            return Ok(output);       
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            IActionResult result;
            try
            {
                result = TryDelete(id);
            }
            catch (SportNotFoundException e) {
                result = NotFound(e.Message);
            }
            return result;
        }

        private IActionResult TryDelete(int id)
        {
            sports.Delete(id);
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SportModelIn modelIn) {
            IActionResult result;
            if (ModelState.IsValid)
            {
                result = ModifyOrAdd(id, modelIn);
            }
            else {
                result = BadRequest(ModelState);
            }
            return result;
        }

        private IActionResult ModifyOrAdd(int id, SportModelIn modelIn)
        {
            IActionResult result;
            Sport toAdd = new Sport(id, modelIn.Name);
            try {
                sports.Modify(toAdd);
                result = Ok();
            }
            catch (SportNotFoundException) {
                sports.Add(toAdd);
                SportModelOut modelOut = new SportModelOut() { Id = id, Name = toAdd.Name };
                result = CreatedAtRoute("GetById", new { id = modelOut.Id }, modelOut);
            }
            return result;
        }
    }
}