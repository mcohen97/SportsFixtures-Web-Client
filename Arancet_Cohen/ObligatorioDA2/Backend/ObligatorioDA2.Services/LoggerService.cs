using System;
using System.Collections.Generic;
using System.Linq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services
{
    public class LoggerService : ILoggerService
    {
        private ILogInfoRepository logRepo;

        public LoggerService(ILogInfoRepository logRepo)
        {
            this.logRepo = logRepo;
        }

        public void Delete(int id)
        {
            logRepo.Delete(id);
        }

        public bool Exists(int id)
        {
            return logRepo.Exists(id);
        }

        public ICollection<LogInfoDto> GetAllLogs()
        {
            return logRepo.GetAll()
                .Select(l => CreateDto(l))
                .ToList();
        }

        public LogInfoDto GetLog(int id)
        {
            LogInfo stored = logRepo.Get(id);
            return CreateDto(stored);
        }

        public int Log(string logType, string messagge, string username, DateTime date)
        {
            LogInfo newLog = new LogInfo()
            {
                Id = 0,
                LogType = logType,
                Message = messagge,
                Username = username,
                Date = date
            };
            try
            {
                newLog = logRepo.Add(newLog);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return newLog.Id;
        }

        private LogInfoDto CreateDto(LogInfo log) {
            LogInfoDto dto = new LogInfoDto()
            {
                id = log.Id,
                date = log.Date,
                logType = log.LogType,
                message = log.Message,
                username = log.Username
            };
            return dto;
        }
    }
}