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
            MatchEntity entity = mapper.ToEntity(aMatch);
            context.Matches.Add(entity);
            context.Update(entity.HomeTeam);
            context.Update(entity.AwayTeam);
            foreach (CommentEntity comment in entity.Commentaries) {
                context.Update(comment);
            }
            context.SaveChanges();
        }



        public void Clear()
        {
            foreach (MatchEntity match in context.Matches) {
                context.Matches.Remove(match);
            }
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Match record)
        {
            throw new NotImplementedException();
        }

        public Match Get(int anId)
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
            MatchEntity toAdd = mapper.ToEntity(aMatch);
            context.Entry(toAdd).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
