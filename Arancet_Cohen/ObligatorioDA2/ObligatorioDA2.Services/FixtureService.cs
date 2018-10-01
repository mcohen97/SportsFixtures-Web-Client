using System;
using System.Collections.Generic;
using BusinessLogic;
using DataRepositoryInterfaces;
using System.Linq;
using ObligatorioDA2.Services.Exceptions;

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
            try
            {
                AddMatches(ref added, generatedMatches);
            }
            catch (TeamAlreadyHasMatchException e)
            {

                RollBack(added);
                throw new WrongFixtureException(e.Message);
            }
            
            return added;
            
        }

        private void RollBack(ICollection<Match> added)
        {
            foreach (Match match in added)
            {
                matchService.DeleteMatch(match.Id);
            }
        }

        private ICollection<Match> AddMatches(ref ICollection<Match> added, ICollection<Match> generated)
        {
            foreach (Match match in generated)
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