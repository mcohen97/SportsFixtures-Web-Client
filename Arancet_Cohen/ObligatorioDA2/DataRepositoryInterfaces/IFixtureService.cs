using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositoryInterfaces
{
    public interface IFixtureService
    {
        FixtureGenerator FixtureAlgorithm { get; set; }
        ICollection<Match> AddFixture(ICollection<Team> teamsCollection);
        ICollection<Match> AddFixture(ICollection<string> teamsNames, string sportName);
        ICollection<Match> AddFixture(Sport sport);
    }
}
