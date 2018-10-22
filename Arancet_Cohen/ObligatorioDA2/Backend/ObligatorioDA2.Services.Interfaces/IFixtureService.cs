using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IFixtureService
    {
        IFixtureGenerator FixtureAlgorithm { get; set; }
        ICollection<Match> AddFixture(ICollection<Team> teamsCollection);
        ICollection<Match> AddFixture(ICollection<string> teamsNames, string sportName);
        ICollection<Match> AddFixture(Sport sport);
    }
}
