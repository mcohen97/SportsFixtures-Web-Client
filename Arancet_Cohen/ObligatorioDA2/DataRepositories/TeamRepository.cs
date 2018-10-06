using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataRepositoryInterfaces;
using DataAccess;
using BusinessLogic;
using RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.DataAccess.Entities;
using ObligatorioDA2.DataAccess.Domain.Mappers;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;


namespace DataRepositories
{
    public class TeamRepository : ITeamRepository
    {
        private DatabaseConnection context;
        private readonly TeamMapper mapper;

        public TeamRepository(DatabaseConnection context)
        {
            this.context = context;
            this.mapper = new TeamMapper();
        }

        public void Clear()
        {
            foreach (TeamEntity team in context.Teams)
            {
                context.Teams.Remove(team);
            }
            context.SaveChanges();
        }


        public Team Get(string sportName, string teamName)
        {
            Team fetched;
            if (Exists(sportName,teamName)) {
                fetched = TryGet(sportName, teamName);
            }else{
                throw new TeamNotFoundException();
            }
        
            return fetched;
        }

        private Team TryGet(string sportName, string teamName)
        {
            TeamEntity entity = context.Teams
                .FirstOrDefault(e => e.Name.Equals(teamName) && e.Sport.Name.Equals(sportName));
            Team converted = mapper.ToTeam(entity);
            return converted;
        }

        public void Delete(string sportName, string teamName)
        {
            if (Exists(sportName, teamName))
            {
                DeleteValid(sportName, teamName);
            }
            else {
                throw new TeamNotFoundException();
            }
        }

        private void DeleteValid(string sportName, string teamName)
        {
            TeamEntity toDelete = context.Teams
                .First(t => t.Sport.Name.Equals(sportName) && t.Name.Equals(teamName));
            context.Teams.Remove(toDelete);
            DeleteMatches(toDelete);
            context.SaveChanges();
        }

        private void DeleteMatches(TeamEntity toDelete)
        {
            IQueryable<MatchEntity> teamMatches = context.Matches
                .Include(m => m.Commentaries)
                .Where(m => m.AwayTeam.Equals(toDelete) || m.HomeTeam.Equals(toDelete));
            context.Matches.RemoveRange(teamMatches);
            foreach (MatchEntity match in teamMatches)
            {
                context.Comments.RemoveRange(match.Commentaries);
            }
        }

        public Team Add( Team aTeam)
        {
            TeamEntity entity = mapper.ToEntity(aTeam);
            Team teamAdded;
            if (!Exists(entity.Sport.Name,entity.Name))
            {
                teamAdded = TryAdd(aTeam);
            }
            else {
                throw new TeamAlreadyExistsException();
            }
            return teamAdded;
        }

        private Team TryAdd(Team aTeam)
        {
            TeamEntity toStore = mapper.ToEntity(aTeam);
            context.Teams.Add(toStore);
            if (context.Sports.Contains(toStore.Sport)) {
                context.Entry(toStore.Sport).State = EntityState.Unchanged;
            }
            context.SaveChanges();
            return mapper.ToTeam(toStore);

        }

        public void Modify(Team aTeam)
        {
            try
            {
                TryModify(aTeam);
            }
            catch (DbUpdateException)
            {
                throw new TeamNotFoundException();
            }
        }

        private void TryModify(Team aTeam)
        {
            TeamEntity entity = mapper.ToEntity(aTeam);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public bool IsEmpty()
        {
            return !context.Teams.Any();
        }

        public bool Exists(string sportName, string teamName)
        {
            return context.Teams.Any(t => t.Sport.Name.Equals(sportName) && t.Name.Equals(teamName));
        }

        public ICollection<Team> GetAll()
        {
            IQueryable<TeamEntity> teams = context.Teams;
            ICollection<Team> result = teams.Select(t => mapper.ToTeam(t)).ToList();
            return result;
        }

        public ICollection<Team> GetFollowedTeams(string username)
        {
            IQueryable<UserTeam> relationships= context.UserTeams
                .Where(ut => ut.UserEntityUserName.Equals(username));
            IQueryable<TeamEntity> teams = relationships.Select(ut => ut.Team);
            return teams.Select(t => mapper.ToTeam(t)).ToList();
        }

        public ICollection<Team> GetTeams(string sportName)
        {
            if (!context.Sports.Any(s => s.Name.Equals(sportName)))
            {
                throw new SportNotFoundException();
            }
            return GetAll().Where(t => t.Sport.Name == sportName).ToList();
        }

        public bool Exists(int id)
        {
            return context.Teams.Any(t => t.Identity == id);
        }

        public void Delete(int id)
        {
            if (!Exists(id))
                throw new TeamNotFoundException();

            TeamEntity toDelete = context.Teams
                .First(t => t.Identity == id);
            context.Teams.Remove(toDelete);
            DeleteMatches(toDelete);
            context.SaveChanges();
        }

        public Team Get(int id)
        {
            if (!Exists(id))
                throw new TeamNotFoundException();

            TeamEntity entity = context.Teams
                .FirstOrDefault(e => e.Identity == id);
            Team converted = mapper.ToTeam(entity);
            return converted ;
        }
    }
}