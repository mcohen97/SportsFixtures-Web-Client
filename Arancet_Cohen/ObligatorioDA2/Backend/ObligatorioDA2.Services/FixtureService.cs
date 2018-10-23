using System;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using System.Linq;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.BusinessLogic.Exceptions;
using System.Reflection;

namespace ObligatorioDA2.Services
{
    public class FixtureService : IFixtureService
    {
        private MatchService matchService;
        private ITeamRepository teamStorage;
        private ISportRepository sportsStorage;
        private IFixtureGenerator fixtureAlgorithm;

        public FixtureService(IMatchRepository matchStorage, ITeamRepository teamRepository, ISportRepository sportsRepository)
        {
            matchService = new MatchService(matchStorage, teamRepository, sportsRepository);
            sportsStorage = sportsRepository;
            teamStorage = teamRepository;
            //fixtureAlgorithm = new OneMatchFixture(DateTime.Now, 2, 5);
        }

        public IFixtureGenerator FixtureAlgorithm { get => fixtureAlgorithm; set => SetFixtureAlgorithm(value); }

        private void SetFixtureAlgorithm(IFixtureGenerator algorithm)
        {
            fixtureAlgorithm = algorithm ?? throw new ArgumentNullException();
        }

        private void RollBack(ICollection<Match> added)
        {
            foreach (Match match in added)
            {
                matchService.DeleteMatch(match.Id);
            }
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

        public ICollection<Match> AddFixture(ICollection<Team> teamsCollection)
        {
            ICollection<Match> added = new List<Match>();
            try
            {
                ICollection<Match> generatedMatches = fixtureAlgorithm.GenerateFixture(teamsCollection);
                AddMatches(ref added, generatedMatches);
            }
            catch (TeamAlreadyHasMatchException e)
            {
                RollBack(added);
                throw new WrongFixtureException(e.Message);
            }
            catch (InvalidTeamCountException e)
            {
                RollBack(added);
                throw new WrongFixtureException(e.Message);
            }

            return added;

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

        public ICollection<Type> GetAlgorithms(string dllPath)
        {
            Assembly myAssembly = Assembly.LoadFile(dllPath);

            ICollection<Type> algorithms = new List<Type>();
            foreach (Type aType in myAssembly.GetTypes())
            {
                //inefficient, but the only way it worked.
                ICollection<string> interfaces = aType.GetInterfaces().Select(i => i.ToString()).ToList();
                bool isElegible = interfaces.Contains(typeof(IFixtureGenerator).ToString());

                if (isElegible) {
                    algorithms.Add(aType);
                }
            }
            return algorithms;
        }
    }
}