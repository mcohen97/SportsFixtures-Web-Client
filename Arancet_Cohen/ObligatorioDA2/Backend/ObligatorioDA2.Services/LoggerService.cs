using System;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Interfaces;

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

        public ICollection<LogInfo> GetAllLogs()
        {
            return logRepo.GetAll();
        }

        public LogInfo GetLog(int id)
        {
            return logRepo.Get(id);
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
            newLog = logRepo.Add(newLog);
            return newLog.Id;
        }
    }
}