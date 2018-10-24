using ObligatorioDA2.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface ILoggerService
    {
        int Log(string logType, string messagge, string username, DateTime date);
        LogInfo GetLog(int id);
        ICollection<LogInfo> GetAllLogs();
    }
}
