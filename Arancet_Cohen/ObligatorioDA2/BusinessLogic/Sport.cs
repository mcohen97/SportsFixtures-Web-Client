using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class Sport
    {
        private string name;
        private ICollection<Team> teams;

        public Sport(string name)
        {
            this.name = name;
            teams = new List<Team>();
        }

        public Sport()
        {
            name = "";
            teams = new List<Team>();
        }

        public void Add(Team team)
        {
            teams.Add(team);
        }

        public bool HasTeam(Team team)
        {
            return teams.Contains(team);
        }

        public void Remove(Team team)
        {
            teams.Remove(team);
        }

        public ICollection<Team> GetTeams()
        {
            ICollection<Team> returnedTeams = new List<Team>(teams);
            return returnedTeams;
        }

        public void Add(ICollection<Team> teamsToAdd)
        {
            foreach (Team team in teamsToAdd)
            {
                this.Add(team);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Sport))
                return false;

            var sport = obj as Sport;
            return name == sport.name;
        }
    }
}