using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using System.Linq;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace ObligatorioDA2.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository matchesStorage;
        private ITeamRepository teamsStorage;
        private ISportRepository sportsStorage;
        private IUserRepository usersStorage;

        public MatchService(IMatchRepository matchsRepository, ITeamRepository teamsRepository, ISportRepository sportsRepository)
        {
            matchesStorage = matchsRepository;
            teamsStorage = teamsRepository;
            sportsStorage = sportsRepository;
        }

        public MatchService(IMatchRepository matchsRepository, ITeamRepository teamsRepository, ISportRepository sportsRepository, IUserRepository usersRepository)
            : this(matchsRepository, teamsRepository, sportsRepository)
        {
            usersStorage = usersRepository;
        }

        public Encounter AddMatch(Encounter aMatch)
        {
            ValidateDate(aMatch);
            return matchesStorage.Add(aMatch);
        }
        public void ModifyMatch(Encounter aMatch)
        {
            ValidateDate(aMatch);
            matchesStorage.Modify(aMatch);
        }

        private void ValidateDate(Encounter aMatch) {
            if (aMatch.GetParticipants().Any(t => DateOccupied(aMatch.Id, t, aMatch.Date)))
            {
                Team occupied = aMatch.GetParticipants().First(t => DateOccupied(aMatch.Id, t, aMatch.Date));
                throw new TeamAlreadyHasMatchException(occupied.Name + " already has a match on date " + aMatch.Date);
            }
        }

        private bool DateOccupied(int matchId, Team team, DateTime date)
        {
            if (matchId > 0)
            {
                return matchesStorage.GetAll().Any(m => (m.GetParticipants().Any(t => t.Equals(team))) && SameDates(m.Date, date));
            }
            else
            {
                return matchesStorage.GetAll().Any(m => (m.Id != matchId) && (m.GetParticipants().Any(t => t.Equals(team))) && SameDates(m.Date, date));
            }
        }

        private bool SameDates(DateTime date1, DateTime date2)
        {
            bool sameYear = date1.Year == date2.Year;
            bool sameMonth = date1.Month == date2.Month;
            bool sameDay = date1.Day == date2.Day;
            return sameYear && sameMonth && sameDay;
        }

        public ICollection<Encounter> GetAllMatches()
        {
            return matchesStorage.GetAll();
        }

        public Encounter GetMatch(int anId)
        {
            return matchesStorage.Get(anId);
        }

        public void DeleteMatch(int anId)
        {
            matchesStorage.Delete(anId);
        }

        public ICollection<Encounter> GetAllMatches(string sportName)
        {
            if (!sportsStorage.Exists(sportName))
            {
                throw new SportNotFoundException();
            }
            return matchesStorage.GetAll().Where(m => m.Sport.Name.Equals(sportName)).ToList();
        }

        public ICollection<Encounter> GetAllMatches(int idTeam)
        {
            if (!teamsStorage.Exists(idTeam))
            {
                throw new TeamNotFoundException();
            }
            return matchesStorage.GetAll().Where(m => m.GetParticipants().Any(t => t.Id == idTeam)).ToList();
        }

        public bool Exists(int id)
        {
            return matchesStorage.Exists(id);
        }

        public Encounter AddMatch(ICollection<int> teamsIds, string sportName, DateTime date)
        {
            return AddMatch(0, teamsIds, sportName, date);
        }

        public Encounter AddMatch(int idMatch, ICollection<int> teamsIds, string sportName, DateTime date)
        {
            ICollection<Team> playingTeams = GetTeams(teamsIds);
            Sport played = sportsStorage.Get(sportName);
            Match toAdd = new Match(idMatch, playingTeams, date, played);
            return AddMatch(toAdd);
        }

        public void ModifyMatch(int idMatch, ICollection<int> teamsIds, DateTime date, string sportName)
        {
            ICollection<Team> playingTeams = GetTeams(teamsIds);
            Sport played = sportsStorage.Get(sportName);
            Match toModify = new Match(idMatch, playingTeams, date, played);
            ModifyMatch(toModify);
        }

        private ICollection<Team> GetTeams(ICollection<int> teamsIds) {
            ICollection<Team> playingTeams = new List<Team>();
            foreach (int teamId in teamsIds)
            {
                Team fetched = teamsStorage.Get(teamId);
                playingTeams.Add(fetched);
            }
            return playingTeams;
        }


        public Commentary CommentOnMatch(int matchId, string userName, string text)
        {
            User commentarist = usersStorage.Get(userName);
            return matchesStorage.CommentOnMatch(matchId, new Commentary(text, commentarist));
        }

        public ICollection<Commentary> GetMatchCommentaries(int matchId)
        {
            Encounter stored = GetMatch(matchId);
            return stored.GetAllCommentaries();
        }

        public ICollection<Commentary> GetAllCommentaries()
        {
            return matchesStorage.GetComments();
        }

        public Commentary GetComment(int id)
        {
            return matchesStorage.GetComment(id);
        }
    }
}
