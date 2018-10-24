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
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<LogInfo> GetAllLogs()
        {
            throw new NotImplementedException();
        }

        public LogInfo GetLog(int id)
        {
            throw new NotImplementedException();
        }

        public int Log(string logType, string messagge, string username, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}