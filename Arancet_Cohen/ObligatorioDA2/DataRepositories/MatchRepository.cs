using BusinessLogic;
using DataAccess;
using DataRepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ObligatorioDA2.DataAccess.Domain.Mappers;
using ObligatorioDA2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace DataRepositories
{
    public class MatchRepository : IMatchRepository
    {
        private DatabaseConnection context;
        private MatchMapper mapper;
        public MatchRepository(DatabaseConnection aContext)
        {
            context = aContext;
            mapper = new MatchMapper();
        }

        public void Add(Match aMatch)
        {
            if (!Exists(aMatch))
            {
                AddNewMatch(aMatch);
            }
            else {
                throw new MatchAlreadyExistsException();
            }
        }

        private void AddNewMatch(Match aMatch)
        {
            MatchEntity entity = mapper.ToEntity(aMatch);
            context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();
        }

        public void Clear()
        {
            foreach (MatchEntity match in context.Matches) {
                context.Matches.Remove(match);
            }
            context.SaveChanges();
        }

        public void Delete(int anId)
        {
            if (AnyWithId(anId))
            {
                DeleteExistent(anId);
            }
            else
            {
                throw new MatchNotFoundException();
            }
        }

        private void DeleteExistent(int anId)
        {
            MatchEntity retrieved = context.Matches.First(m => m.Id == anId);
            context.Matches.Remove(retrieved);
            context.SaveChanges();
        }

        public bool Exists(Match record)
        {
            return AnyWithId(record.Id);
        }

        public Match Get(int anId)
        {
            Match toReturn;
            if (AnyWithId(anId))
            {
                toReturn = GetExistentMatch(anId);
            }
            else {
                throw new MatchNotFoundException();
            }
            return toReturn;
        }

        private Match GetExistentMatch(int anId)
        {
            MatchEntity a = context.Matches.First(me => me.Id == anId);
            Match conversion = mapper.ToMatch(a);
            return conversion;
        }

        public ICollection<Match> GetAll()
        {
            IQueryable<MatchEntity> entities = context.Matches;
            ICollection<Match> translation = entities.Select(m => mapper.ToMatch(m)).ToList();
            return translation;
        }

        public bool IsEmpty()
        {
            return !context.Matches.Any();
        }

        public void Modify(Match aMatch)
        {
            if (Exists(aMatch))
            {
                ModifyExistent(aMatch);
            }
            else {
                throw new MatchNotFoundException();
            }
        }

        private void ModifyExistent(Match aMatch)
        {
            MatchEntity toAdd = mapper.ToEntity(aMatch);
            context.Entry(toAdd).State = EntityState.Modified;
            context.SaveChanges();
        }

        private bool AnyWithId(int anId) {
            return context.Matches.Any(m => m.Id==anId);
        }
    }
}
