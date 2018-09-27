using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public abstract class FixtureGenerator
    {
        public abstract DateTime InitialDate { get; set; }
        public abstract int RoundLength { get; set; }
        public abstract int DaysBetweenRounds { get; set; }

        public abstract ICollection<Match> GenerateFixture(ICollection<Team> teams);
    }
}