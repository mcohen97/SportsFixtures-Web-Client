using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.Data.DataAccess;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.DataAccess.Domain.Mappers;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ObligatorioDA2.Data.Repositories.Interfaces;

namespace ObligatorioDA2.Data.Repositories
{
    public class SportRepository :ISportRepository
    {
        private DatabaseConnection context;
        private SportMapper mapper;

        public SportRepository(DatabaseConnection context)
        {
            this.context = context;
            mapper = new SportMapper();
        }

        public Sport Add(Sport sport)
        {
            if (Exists(sport.Name))
                throw new SportAlreadyExistsException();

            SportEntity entity = mapper.ToEntity(sport);
            context.Entry(entity).State = EntityState.Added;
            context.SaveChanges();
            return mapper.ToSport(entity);
        }

        public void Clear()
        {
            foreach (SportEntity sport in context.Sports)
            {
                context.Sports.Remove(sport);
            }
            context.SaveChanges();
        }

        public void Delete(string name)
        {
            if (!Exists(name))
            {
                throw new SportNotFoundException();
            }
            SportEntity sportInDb = context.Sports.First(s => s.Name == name);
            context.Sports.Remove(sportInDb);
            DeleteTeamsMatches(name);
            context.SaveChanges();
        }

        private void DeleteTeamsMatches(string sportName)
        {
            IQueryable<TeamEntity> teams = context.Teams.Where(t => t.SportEntityName.Equals(sportName));
            context.Teams.RemoveRange(teams);
            foreach (TeamEntity deleted in teams) {
                IQueryable<MatchEntity> played = context.Matches.Include(m => m.Commentaries)
                    .Where(m => (m.HomeTeam.Identity == deleted.Identity) || (m.AwayTeam.Identity == deleted.Identity));
                context.Matches.RemoveRange(played);
                IQueryable<UserTeam> followings = context.UserTeams.Where(t => t.Team.Identity == deleted.Identity);
                context.UserTeams.RemoveRange(followings);
                foreach (MatchEntity match in played) {
                    context.Comments.RemoveRange(match.Commentaries);
                }
            }
            
        }

        public bool Exists(string name)
        {
            return context.Sports.Any(s => s.Name == name);
        }
        public ICollection<Sport> GetAll()
        {
            IQueryable<Sport> query = context.Sports.Select(s => mapper.ToSport(s));
            return query.ToList();
        }

        public bool IsEmpty()
        {
            return !context.Sports.Any();
        }

        public void Modify(Sport entity)
        {
            if (!Exists(entity.Name))
                throw new SportNotFoundException();

            SportEntity modified = mapper.ToEntity(entity);
            context.Entry(modified).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Sport Get(string name)
        {
            if (!Exists(name))
            {
                throw new SportNotFoundException();
            }
            SportEntity sportInDb = context.Sports.First(s => s.Name == name);
            return mapper.ToSport(sportInDb);
        }

 
    }
}
