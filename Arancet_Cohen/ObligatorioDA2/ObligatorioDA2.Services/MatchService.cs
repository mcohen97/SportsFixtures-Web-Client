using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using DataRepositoryInterfaces;
using System.Linq;
using ObligatorioDA2.Services.Exceptions;
using BusinessLogic.Exceptions;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace ObligatorioDA2.Services
{
    public class MatchService: IMatchService
    {
        private IMatchRepository matchesStorage;
        private ITeamRepository teamsStorage;
        private ISportRepository sportsStorage;

        public MatchService(IMatchRepository matchsRepository, ITeamRepository teamsRepository, ISportRepository sportsRepository)
        {
            matchesStorage = matchsRepository;
            teamsStorage = teamsRepository;
            sportsStorage = sportsRepository;
        }

        public Match AddMatch(Match aMatch)
        {

            if (DateOccupied(aMatch.HomeTeam, aMatch.Date))
                throw new TeamAlreadyHasMatchException(aMatch.HomeTeam + "already has a match on date" + aMatch.Date);
            if (DateOccupied(aMatch.AwayTeam, aMatch.Date))
                throw new TeamAlreadyHasMatchException(aMatch.HomeTeam + "already has a match on date" + aMatch.Date);

            return matchesStorage.Add(aMatch);
        }

        private bool DateOccupied(Team team, DateTime date)
        {
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
            return matchesStorage.GetAll().Where(m => m.Sport.Equals(sport)).ToList();
        }

        public ICollection<Match> GetAllMatches(Team team)
        {
            return matchesStorage.GetAll().Where(m => m.HomeTeam.Equals(team) || m.AwayTeam.Equals(team)).ToList();
        }

        public bool Exists(int id)
        {
            return matchesStorage.Exists(id);
        }

        public Match AddMatch(int homeTeamId, int awayTeamId ,string sportName,DateTime date)
        {
            return AddMatch(0, homeTeamId,awayTeamId,sportName,date);
        }

        public Match AddMatch(int idMatch, int homeTeamId, int awayTeamId, string sportName, DateTime date)
        {
            Team home = teamsStorage.Get(homeTeamId);
            Team away = teamsStorage.Get(awayTeamId);
            Sport played = sportsStorage.Get(sportName);
            Match toAdd = new Match(idMatch,home, away, date, played);
            return AddMatch(toAdd);
        }

        public void ModifyMatch(int idMatch, int idHome, int idAway, DateTime date, string sportName)
        {
            Team home = teamsStorage.Get(idHome);
            Team away = teamsStorage.Get(idAway);
            Sport played = sportsStorage.Get(sportName);
            Match toModify = new Match(idMatch, home, away, date, played);
            ModifyMatch(toModify);
        }

        public void CommentOnMatch(Match aMatch, Commentary aComment)
        {
            try
            {
                aMatch.AddCommentary(aComment);
                ModifyMatch(aMatch);
            }
            catch (InvalidMatchDataExcpetion) {
                throw new CommentAlreadyExistsException();
            }
        }

        public void CommentOnMatch(int matchId, string userName, string text)
        {
            throw new NotImplementedException();
        }
    }
}
