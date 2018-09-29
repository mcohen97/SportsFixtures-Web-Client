using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BusinessLogic.Exceptions;

[assembly:InternalsVisibleTo("BusinessLogicTest")]
[assembly:InternalsVisibleTo("DataRepositoriesTest")]

namespace BusinessLogic
{
    public class Sport
    {
        private string name;
        private ICollection<Team> teams;
        public int Id { get; private set; }

        public string Name { get{return name;} set{SetName(value);} }

        private void SetName(string value)
        {
            if(value == null)
                throw new InvalidSportDataException("Name can't be null");
            
            if(value == "")
                throw new InvalidSportDataException("Name can't be empty");

            name = value;
        }

        public Sport(string name)
        {
            this.name = name;
            teams = new List<Team>();
        }


        public Sport(string name, ICollection<Team> someTeams):this(name)
        {
            teams = someTeams;
        }

        public void AddTeam(Team team)
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
                this.AddTeam(team);
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