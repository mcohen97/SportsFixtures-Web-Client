using System;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using System.Linq;

namespace BusinessLogic
{
    public class Match
    {
        private Team homeTeam;
        private Team awayTeam;
        private DateTime date;
        private ICollection<Commentary> commentaries;

        public Match(Team home, Team away, DateTime date)
        {
            HomeTeam = home;
            AwayTeam = away;
            Date = date;
            commentaries = new List<Commentary>();
        }

        public Team HomeTeam { get{return homeTeam;} set{SetHomeTeam(value);} }
        public Team AwayTeam { get{return awayTeam;} set{SetAwayTeam(value);} }
        public DateTime Date { get{return date;} set{SetDate(value);} }

        public bool HasCommentary(Commentary commentary)
        {
            return commentaries.Contains(commentary);
        }

        public void AddCommentary(Commentary commentary)
        {
            if(HasCommentary(commentary))
                throw new InvalidMatchDataExcpetion("Commentary already exists in this match");
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

        private void SetHomeTeam(Team value)
        {
            if(value == null)
                throw new InvalidMatchDataExcpetion("Home team can't be null");
            if(value.Equals(awayTeam))
                throw new InvalidMatchDataExcpetion("Home team can't be same as away team");

            homeTeam = value;
        }

        private void SetAwayTeam(Team value)
        {
            if(value == null)
                throw new InvalidMatchDataExcpetion("Away team can't be null");
            if(value.Equals(homeTeam))
                throw new InvalidMatchDataExcpetion("Away team can't be same as home team");
                
            awayTeam = value;
        }

        private void SetDate(DateTime value)
        {
            date = value;
        }

        public Commentary GetCommentary(int id)
        {
            return commentaries.First(c => c.Id == id);
        }
    }
}