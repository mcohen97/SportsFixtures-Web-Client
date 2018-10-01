using System;
using System.Collections.Generic;
using BusinessLogic;
using DataRepositoryInterfaces;
using System.Linq;

namespace ObligatorioDA2.Services
{
    public class FixtureService
    {
        private MatchService matchService;
        private ITeamRepository teamStorage;
        private FixtureGenerator fixtureAlgorithm;

        public FixtureService(IMatchRepository matchStorage, ITeamRepository teamStorage)
        {
            this.matchService = new MatchService(matchStorage);
            this.teamStorage = teamStorage;
            fixtureAlgorithm = new OneMatchFixture(DateTime.Now, 2, 5);
        }

        public FixtureGenerator FixtureAlgorithm { get => fixtureAlgorithm; set => SetFixtureAlgorithm(value); }

        private void SetFixtureAlgorithm(FixtureGenerator algorithm)
        {
            fixtureAlgorithm = algorithm ?? throw new ArgumentNullException();
        }

        public ICollection<Match> AddFixture(ICollection<Team> teamsCollection)
        {
            ICollection<Match> added = new List<Match>();
            ICollection<Match> generatedMatches = fixtureAlgorithm.GenerateFixture(teamsCollection);
            foreach(Match match in generatedMatches)
            {
                int id = matchService.AddMatch(match);
                added.Add(new Match(id, match.HomeTeam, match.AwayTeam, match.Date, match.Sport));
            }
            return added;
            
        }

        public ICollection<Match> AddFixture(ICollection<string> teamsNames, string sportName)
        {
            ICollection<Team> teamsCollection = teamsNames.Select(name => teamStorage.Get(sportName, name)).ToList();
            return AddFixture(teamsCollection);
        }

    }
}