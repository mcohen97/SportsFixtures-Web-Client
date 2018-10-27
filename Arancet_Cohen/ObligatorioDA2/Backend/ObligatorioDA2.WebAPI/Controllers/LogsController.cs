using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.WebAPI.Models;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private ILoggerService logger;

        public LogsController(ILoggerService logService)
        {
            logger = logService;
        }

        [HttpGet]
        [Authorize(Roles = AuthenticationConstants.ADMIN_ROLE)]
        public IActionResult GetAll()
        {
            IActionResult result;
            try
            {
                result = TryGet();
            }
            catch (DataInaccessibleException e)
            {

                result = NoDataAccess(e);
            }

            return result;
        }

        private IActionResult TryGet()
        {
            ICollection<LogInfo> logs = logger.GetAllLogs();
            IEnumerable<LogModelOut> output = logs.Select(l => new LogModelOut {
                Id = l.Id,
                Date = l.Date,
                LogType = l.LogType,
                Message = l.Message,
                Useranme = l.Username
            });
            return Ok(output);
        }

        private IActionResult NoDataAccess(DataInaccessibleException e)
        {
            ErrorModelOut error = new ErrorModelOut() { ErrorMessage = e.Message };
            IActionResult internalError = StatusCode((int)HttpStatusCode.InternalServerError, error);
            return internalError;
        }
    }
}