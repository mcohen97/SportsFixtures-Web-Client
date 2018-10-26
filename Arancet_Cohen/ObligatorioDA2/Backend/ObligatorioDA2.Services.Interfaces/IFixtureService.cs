using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IFixtureService
    {
        IFixtureGenerator FixtureAlgorithm { get; set; }
        ICollection<Encounter> AddFixture(ICollection<Team> teamsCollection);
        ICollection<Encounter> AddFixture(ICollection<string> teamsNames, string sportName);
        ICollection<Encounter> AddFixture(Sport sport);
        ICollection<Type> GetAlgorithms(string dllPath);
    }
}
