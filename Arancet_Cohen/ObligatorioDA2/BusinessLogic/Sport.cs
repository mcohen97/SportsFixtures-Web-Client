using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class Sport
    {
        private string name;

        public Sport(string name)
        {
            this.name = name;
        }

        public Sport()
        {
        }

        public void Add(Team team)
        {
            throw new NotImplementedException();
        }

        public bool HasTeam(Team team)
        {
            throw new NotImplementedException();
        }

        public void Remove(Team team)
        {
            throw new NotImplementedException();
        }

        public ICollection<Team> GetTeams()
        {
            throw new NotImplementedException();
        }

        public void Add(ICollection<Team> teams)
        {
            throw new NotImplementedException();
        }
    }
}