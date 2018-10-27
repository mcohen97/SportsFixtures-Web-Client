using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public interface LogService
    {
        int Log(LogInfo logInfo);
        LogInfo GetLogInfo(int id);
        ICollection<LogInfo> GetAll();
    }
}
