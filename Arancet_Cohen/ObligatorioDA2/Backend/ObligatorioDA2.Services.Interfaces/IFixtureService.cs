using System;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IFixtureService
    {
        IFixtureGenerator FixtureAlgorithm { get; set; }
        ICollection<EncounterDto> AddFixture(ICollection<string> teamsNames, string sportName);
        ICollection<EncounterDto> AddFixture(string sportName);
        ICollection<Type> GetAlgorithms(string dllPath);
    }
}
