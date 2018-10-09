using System.Collections.Generic;
using ObligatorioDA2.Data.DataAccess;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.DataAccess.Domain.Mappers;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ObligatorioDA2.Data.Repositories.Interfaces;
using System;
using System.Data.Common;

namespace ObligatorioDA2.Data.Repositories
{
    public class SportRepository : ISportRepository
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
            Sport added;
            try
            {
                added = TryAdd(sport);
            }
            catch (DbException) {
                throw new DataInaccessibleException();
            }
            return added;
        }

        private Sport TryAdd(Sport sport)
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
            try
            {
                TryClear();
            }
            catch (DbException) {
                throw new DataInaccessibleException();
            }
        }

        private void TryClear()
        {
            foreach (SportEntity sport in context.Sports)
            {
                context.Sports.Remove(sport);
            }
            context.SaveChanges();
        }

        public void Delete(string name)
        {
            try
            {
                TryDelete(name);
            }
            catch (DbException) {
                throw new DataInaccessibleException();
            }
        }

        private void TryDelete(string name)
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
            bool exists;
            try
            {
                exists = AskIfExists(name);
            }
            catch (DbException) {
                throw new DataInaccessibleException();
            }
            return exists;
        }

        private bool AskIfExists(string name)
        {
            return context.Sports.Any(s => s.Name == name);
        }

        public ICollection<Sport> GetAll()
        {
            ICollection<Sport> allOfThem;
            try
            {
                allOfThem = TryGetAll();
            }
            catch (DbException) {
                throw new DataInaccessibleException();
            }
            return allOfThem;
        }

        private ICollection<Sport> TryGetAll()
        {
            IQueryable<Sport> query = context.Sports.Select(s => mapper.ToSport(s));
            return query.ToList();
        }

        public bool IsEmpty()
        {
            bool empty;
            try
            {
                empty = AskIfEmpty();
            }
            catch (DbException) {
                throw new DataInaccessibleException();
            }
            return empty;
        }

        private bool AskIfEmpty()
        {
            return !context.Sports.Any();
        }

        public void Modify(Sport entity)
        {
            try
            {
                TryModify(entity);
            }
            catch (DbException) {
                throw new DataInaccessibleException();
            }
        }

        private void TryModify(Sport entity)
        {
            if (!Exists(entity.Name))
                throw new SportNotFoundException();

            SportEntity modified = mapper.ToEntity(entity);
            context.Entry(modified).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Sport Get(string name)
        {
            Sport toGet;
            try
            {
                toGet = TryGet(name);
            }
            catch (DbException) {
                throw new DataInaccessibleException();
            }
            return toGet;
        }

        private Sport TryGet(string name)
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
