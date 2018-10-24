using ObligatorioDA2.Services;
using System;

namespace ObligatorioDA2.Data.DataAccess.Entities
{
    public class LogInfoMapper
    {
        public LogInfoEntity ToEntity(LogInfo log)
        {
            return new LogInfoEntity()
            {
                Id = log.Id,
                Messagge = log.Messagge,
                Date = log.Date,
                LogType = log.LogType,
                Username = log.Username
            };
        }

        public LogInfo ToLogInfo(LogInfoEntity entity)
        {
            return new LogInfo()
            {
                Id = entity.Id,
                Messagge = entity.Messagge,
                Date = entity.Date,
                LogType = entity.LogType,
                Username = entity.Username
            };
        }
    }
}