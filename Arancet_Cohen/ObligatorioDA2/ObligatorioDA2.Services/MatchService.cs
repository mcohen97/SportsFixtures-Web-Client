using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using DataRepositoryInterfaces;
using System.Linq;
using ObligatorioDA2.Services.Exceptions;

namespace ObligatorioDA2.Services
{
    public class MatchService
    {
        private IMatchRepository matchesStorage;

        public MatchService(IMatchRepository matchsRepository)
        {
            matchesStorage = matchsRepository;
        }

        public void AddMatch(Match aMatch)
        {

            if (DateOccupied(aMatch.HomeTeam, aMatch.Date))
                throw new TeamAlreadyHasMatchException(aMatch.HomeTeam + "already has a match on date" + aMatch.Date);
            if (DateOccupied(aMatch.AwayTeam, aMatch.Date))
                throw new TeamAlreadyHasMatchException(aMatch.HomeTeam + "already has a match on date" + aMatch.Date);

            matchesStorage.Add(aMatch);
        }

        private bool DateOccupied(Team team, DateTime date)
        {
            //SHOULD BE A METHOD IN MATCHREPOSITORY THAT EXECUTES THE QUERY IN DB
            return matchesStorage.GetAll().Any(m => (m.HomeTeam.Equals(team) || m.AwayTeam.Equals(team)) && SameDates(m.Date, date));
        }

        private bool SameDates(DateTime date1, DateTime date2)
        {
            bool sameYear = date1.Year == date2.Year;
            bool sameMonth = date1.Month == date2.Month;
            bool sameDay = date1.Day == date2.Day;
            return sameYear && sameMonth && sameDay;
        }

        public ICollection<Match> GetAllMatches()
        {
            return matchesStorage.GetAll();
        }

        public Match GetMatch(int anId)
        {
            return matchesStorage.Get(anId);
        }

        public void DeleteMatch(int anId)
        {
            matchesStorage.Delete(anId);
        }

        public void ModifyMatch(Match aMatch)
        {
            matchesStorage.Modify(aMatch);
        }

        public ICollection<Match> GetAllMatches(Sport sport)
        {
            //SHOULD BE A METHOD IN MATCHREPOSITORY THAT EXECUTES THE QUERY IN DB
            return matchesStorage.GetAll().Where(m => m.Sport.Equals(sport)).ToList();
        }

        public ICollection<Match> GetAllMatches(Team team)
        {
            //SHOULD BE A METHOD IN MATCHREPOSITORY THAT EXECUTES THE QUERY IN DB
            return matchesStorage.GetAll().Where(m => m.HomeTeam.Equals(team) || m.AwayTeam.Equals(team)).ToList();
        }

        public bool Exists(int id)
        {
            return matchesStorage.Exists(id);
        }
    }
}
