using System;
using System.Collections.Generic;
using BusinessLogic;
using DataRepositoryInterfaces;

namespace ObligatorioDA2.Services
{
    public class FixtureService
    {
        private IMatchRepository matchStorage;
        private ITeamRepository teamStorage;

        public FixtureService(IMatchRepository matchStorage, ITeamRepository teamStorage)
        {
            this.matchStorage = matchStorage;
            this.teamStorage = teamStorage;
        }

        public FixtureGenerator FixtureAlgorithm { get; set; }

        public ICollection<Match> AddFixture(ICollection<Team> teamsCollection)
        {
            throw new NotImplementedException();
        }

        public ICollection<Match> AddFixture(ICollection<string> teamsNames)
        {
            throw new NotImplementedException();
        }

        public ICollection<Match> AddFixture(ICollection<string> teamsNames, string sportName)
        {
            throw new NotImplementedException();
        }
    }
}