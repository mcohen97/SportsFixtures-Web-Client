using System;
using System.Collections.Generic;

namespace ObligatorioDA2.BusinessLogic
{
    public interface IFixtureGenerator
    {
        DateTime InitialDate { get; set; }
        int RoundLength { get; set; }
        int DaysBetweenRounds { get; set; }

        ICollection<Match> GenerateFixture(ICollection<Team> teams);
    }
}