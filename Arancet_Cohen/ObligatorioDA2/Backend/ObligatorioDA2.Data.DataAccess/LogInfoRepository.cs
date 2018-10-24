using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.DomainMappers;
using ObligatorioDA2.Data.Entities;
using ObligatorioDA2.Data.Repositories.Interfaces;

namespace ObligatorioDA2.Data.DataAccess
{
    public class LogInfoRepository : ILogInfoRepository
    {
        private DatabaseConnection context;
        private LogInfoMapper mapper;

        public LogInfoRepository(DatabaseConnection context)
        {
            this.context = context;
            mapper = new LogInfoMapper();
        }

        public LogInfo Add(LogInfo log)
        {
            LogInfo added;
            try
            {
                added = TryAdd(log);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return added;
        }

        private LogInfo TryAdd(LogInfo log)
        {
            if (Exists(log.Id))
                throw new LogAlreadyExistsException();

            LogInfoEntity entity = mapper.ToEntity(log);
            context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();
            return mapper.ToLogInfo(entity);
        }

        public void Clear()
        {
            try
            {
                TryClear();
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
        }

        private void TryClear()
        {
            foreach (LogInfoEntity log in context.Logs)
            {
                context.Logs.Remove(log);
            }
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            try
            {
                TryDelete(id);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
        }

        private void TryDelete(int id)
        {
            if (!Exists(id))
                throw new LogNotFoundException();
            
            LogInfoEntity logInDb = context.Logs.First(l => l.Id == id);
            context.Logs.Remove(logInDb);
            context.SaveChanges();
        }

        public bool Exists(int id)
        {
            bool exists;
            try
            {
                exists = AskIfExists(id);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return exists;
        }

        private bool AskIfExists(int id)
        {
            return context.Logs.Any(l => l.Id == id);
        }

        public LogInfo Get(int id)
        {
            LogInfo toGet;
            try
            {
                toGet = TryGet(id);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return toGet;
        }

        private LogInfo TryGet(int id)
        {
            if (!Exists(id))
                throw new LogNotFoundException();

            LogInfoEntity logInDb = context.Logs.First(l => l.Id == id);
            return mapper.ToLogInfo(logInDb);
        }

        public ICollection<LogInfo> GetAll()
        {
            ICollection<LogInfo> allOfThem;
            try
            {
                allOfThem = TryGetAll();
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return allOfThem;
        }

        private ICollection<LogInfo> TryGetAll()
        {
            IQueryable<LogInfo> query = context.Logs.Select(l => mapper.ToLogInfo(l));
            return query.ToList();
        }

        public bool IsEmpty()
        {
            bool empty;
            try
            {
                empty = AskIfEmpty();
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
            return empty;
        }

        private bool AskIfEmpty()
        {
            return !context.Logs.Any();
        }

        public void Modify(LogInfo entity)
        {
            try
            {
                TryModify(entity);
            }
            catch (DbException)
            {
                throw new DataInaccessibleException();
            }
        }

        private void TryModify(LogInfo entity)
        {
            if (!Exists(entity.Id))
                throw new LogNotFoundException();

            LogInfoEntity modified = mapper.ToEntity(entity);
            context.Entry(modified).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}