using System;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic.Exceptions;
using System.Linq;

namespace ObligatorioDA2.BusinessLogic
{
    public class Match
    {
        private ICollection<Team> participants;
        private DateTime date;
        private Sport sport;
        private ICollection<Commentary> commentaries;
        public int Id { get; private set; }

        public Match(ICollection<Team> teams, DateTime date, Sport aSport)
        {
            Sport = aSport;
            SetTeams(teams);
            Date = date;
            commentaries = new List<Commentary>();
        }

        public void SetTeams(ICollection<Team> teams)
        {
            if (teams == null) {
                throw new InvalidMatchDataException("A match can't be null");
            }
            if (teams.Count < 2) {
                throw new InvalidMatchDataException("A match can't have less than 2 teams");
            }
            if (RepeatedMatches(teams)) {
                throw new InvalidMatchDataException("A match can't have repeated teams");
            }
            if (Sport.IsTwoTeams && teams.Count > 2) {
                throw new InvalidMatchDataException("The sport does not allow more than two teams");
            }
            if (!TeamsPlaySport(teams, Sport)) {
                throw new InvalidMatchDataException("The teams must play the specified sport");
            }
            participants = teams;
        }

        private bool TeamsPlaySport(ICollection<Team> teams, Sport sport)
        {
            return !teams.Any(t => !t.Sport.Equals(sport));
        }

        private bool RepeatedMatches(ICollection<Team> teams)
        {
            return teams.Count != teams.Distinct().Count();
        }

        public Match(int anId, ICollection<Team> teams, DateTime date, Sport sport) : this(teams, date, sport)
        {
            Id = anId;
        }

        public Match(int anId, ICollection<Team> teams, DateTime date, Sport sport, ICollection<Commentary> comments) : this(anId, teams, date, sport)
        {
            commentaries = comments;
        }

        public DateTime Date { get { return date; } set { SetDate(value); } }

        public Sport Sport { get { return sport; } set { SetSport(value); } }

        public ICollection<Team> GetParticipants() {
            return participants;
        }

        public bool HasCommentary(Commentary commentary)
        {
            return commentaries.Contains(commentary);
        }

        public void AddCommentary(Commentary commentary)
        {
            if (HasCommentary(commentary))
                throw new InvalidMatchDataException("Commentary already exists in this match");
            commentaries.Add(commentary);
        }

        public void RemoveCommentary(Commentary commentary)
        {
            commentaries.Remove(commentary);
        }

        public ICollection<Commentary> GetAllCommentaries()
        {
            return new List<Commentary>(commentaries);
        }

        public void RemoveAllComments()
        {
            commentaries.Clear();
        }

        private void SetDate(DateTime value)
        {
            date = value;
        }


        private void SetSport(Sport value)
        {
            if (value == null)
            {
                throw new InvalidMatchDataException();
            }
            sport = value;
        }

        public Commentary GetCommentary(int id)
        {
            return commentaries.First(c => c.Id == id);
        }
    }
}