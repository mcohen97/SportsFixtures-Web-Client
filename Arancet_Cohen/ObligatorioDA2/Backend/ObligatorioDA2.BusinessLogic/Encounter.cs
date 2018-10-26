using ObligatorioDA2.BusinessLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObligatorioDA2.BusinessLogic
{
    public abstract class Encounter
    {
        public int Id { get; private set; }
        private Sport played;
        private DateTime date;
        private ICollection<Team> participants;
        private ICollection<Commentary> commentaries;
        private Result result;
        public Result Result { get { return result; } set { SetResult(value); } }

        public Encounter(ICollection<Team> teams, DateTime date, Sport aSport)
        {
            Sport = aSport;
            SetTeams(teams);
            Date = date;
            commentaries = new List<Commentary>();
        }
        public Encounter(int anId, ICollection<Team> teams, DateTime date, Sport sport) : this(teams, date, sport)
        {
            Id = anId;
        }

        public Encounter(int anId, ICollection<Team> teams, DateTime date, Sport sport,
         ICollection<Commentary> comments) : this(anId, teams, date, sport)
        {
            commentaries = comments;
        }

        public Sport Sport { get { return played; } set { SetSport(value); } }

        private void SetSport(Sport value)
        {
            if (value == null)
            {
                throw new InvalidMatchDataException();
            }
            played = value;
        }
        public DateTime Date { get { return date; } set { SetDate(value); } }

        private void SetDate(DateTime value)
        {
            date = value;
        }

        public void SetTeams(ICollection<Team> teams)
        {
            if (teams == null)
            {
                throw new InvalidMatchDataException("A match can't be null");
            }
            if (teams.Count < 2)
            {
                throw new InvalidMatchDataException("A match can't have less than 2 teams");
            }
            if (RepeatedMatches(teams))
            {
                throw new InvalidMatchDataException("A match can't have repeated teams");
            }
            if (Sport.IsTwoTeams && teams.Count > 2)
            {
                throw new InvalidMatchDataException("The sport does not allow more than two teams");
            }
            if (!TeamsPlaySport(teams, Sport))
            {
                throw new InvalidMatchDataException("The teams must play the specified sport");
            }
            participants = teams;
        }

        public ICollection<Team> GetParticipants()
        {
            return participants;
        }

        private bool TeamsPlaySport(ICollection<Team> teams, Sport sport)
        {
            return !teams.Any(t => !t.Sport.Equals(sport));
        }

        private bool RepeatedMatches(ICollection<Team> teams)
        {
            return teams.Count != teams.Distinct().Count();
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

        public Commentary GetCommentary(int id)
        {
            return commentaries.First(c => c.Id == id);
        }

        public bool HasResult()
        {
            return Result != null;
        }
        public void SetResult(Result aResult)
        {
            result = aResult;
        }
    }
}
