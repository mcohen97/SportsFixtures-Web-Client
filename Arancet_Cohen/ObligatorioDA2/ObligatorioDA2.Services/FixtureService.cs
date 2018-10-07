using System;
using System.Collections.Generic;
using BusinessLogic;
using DataRepositoryInterfaces;
using System.Linq;
using ObligatorioDA2.Services.Exceptions;

namespace ObligatorioDA2.Services
{
    public class FixtureService : IFixtureService
    {
        private MatchService matchService;
        private ITeamRepository teamStorage;
        private ISportRepository sportsStorage;
        private FixtureGenerator fixtureAlgorithm;

        public FixtureService(IMatchRepository matchStorage, ITeamRepository teamRepository,ISportRepository sportsRepository)
        {
            matchService = new MatchService(matchStorage,teamRepository,sportsRepository);
            sportsStorage = sportsRepository;
            teamStorage = teamRepository;
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
                Match matchAdded = matchService.AddMatch(match);
                added.Add(matchAdded);
            }
            return added;
        }

        public ICollection<Match> AddFixture(ICollection<string> teamsNames, string sportName)
        {
            ICollection<Team> teamsCollection = teamsNames.Select(name => teamStorage.Get(sportName, name)).ToList();
            return AddFixture(teamsCollection);
        }

        public ICollection<Match> AddFixture(Sport sport)
        {
            ICollection<Team> teamsCollection = teamStorage.GetAll().Where(t => t.Sport.Equals(sport)).ToList();
            return AddFixture(teamsCollection);
        }
    }
}