using ObligatorioDA2.Services.Interfaces.Dtos;
using System;
using System.Collections.Generic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface ILoggerService
    {
        int Log(string logType, string messagge, string username, DateTime date);
        LogInfoDto GetLog(int id);
        bool Exists(int id);
        void Delete(int id);
        ICollection<LogInfoDto> GetAllLogs();
    }
}
