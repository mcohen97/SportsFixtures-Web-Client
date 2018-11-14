using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;
using ObligatorioDA2.WebAPI.Models;

namespace ObligatorioDA2.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private ILoggerService logger;
        private ErrorActionResultFactory errors;

        public LogsController(ILoggerService logService)
        {
            logger = logService;
            errors = new ErrorActionResultFactory(this);
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
            catch (ServiceException e)
            {
                result = errors.GenerateError(e);
            }

            return result;
        }

        private IActionResult TryGet()
        {
            ICollection<LogInfoDto> logs = logger.GetAllLogs();
            IEnumerable<LogModelOut> output = logs.Select(l => new LogModelOut {
                Id = l.id,
                Date = l.date,
                LogType = l.logType,
                Message = l.message,
                Username = l.username
            });
            return Ok(output);
        }

    }
}