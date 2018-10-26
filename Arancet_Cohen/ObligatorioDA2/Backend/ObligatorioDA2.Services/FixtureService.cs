using System;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using System.Linq;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.BusinessLogic.Exceptions;
using System.Reflection;
using System.IO;

namespace ObligatorioDA2.Services
{
    public class FixtureService : IFixtureService
    {
        private MatchService matchService;
        private ITeamRepository teamStorage;
        private ISportRepository sportsStorage;
        private IFixtureGenerator fixtureAlgorithm;
        private const string DLL_EXTENSION = "*.dll";


        public FixtureService(IMatchRepository matchStorage, ITeamRepository teamRepository, ISportRepository sportsRepository)
        {
            matchService = new MatchService(matchStorage, teamRepository, sportsRepository);
            sportsStorage = sportsRepository;
            teamStorage = teamRepository;
        }

        public IFixtureGenerator FixtureAlgorithm { get => fixtureAlgorithm; set => SetFixtureAlgorithm(value); }

        private void SetFixtureAlgorithm(IFixtureGenerator algorithm)
        {
            fixtureAlgorithm = algorithm ?? throw new ArgumentNullException();
        }

        private void RollBack(ICollection<Encounter> added)
        {
            foreach (Encounter match in added)
            {
                matchService.DeleteMatch(match.Id);
            }
        }

        public ICollection<Encounter> AddFixture(ICollection<string> teamsNames, string sportName)
        {
            ICollection<Team> teamsCollection = teamsNames.Select(name => teamStorage.Get(sportName, name)).ToList();
            return AddFixture(teamsCollection);
        }

        public ICollection<Encounter> AddFixture(Sport sport)
        {
            ICollection<Team> teamsCollection = teamStorage.GetAll().Where(t => t.Sport.Equals(sport)).ToList();
            return AddFixture(teamsCollection);
        }

        public ICollection<Encounter> AddFixture(ICollection<Team> teamsCollection)
        {
            ICollection<Encounter> added = new List<Encounter>();
            try
            {
                ICollection<Encounter> generatedMatches = fixtureAlgorithm.GenerateFixture(teamsCollection);
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
        private ICollection<Encounter> AddMatches(ref ICollection<Encounter> added, ICollection<Encounter> generated)
        {
            foreach (Encounter match in generated)
            {
                Encounter matchAdded = matchService.AddMatch(match);
                added.Add(matchAdded);
            }
            return added;
        }

        public ICollection<Type> GetAlgorithms(string dllPath)
        {
            string[] files = Directory.GetFiles(dllPath, DLL_EXTENSION);
            IEnumerable<Type> interestingTypes = new List<Type>();
            foreach (var file in files)
            {
                Type[] types;
                try
                {
                    types = Assembly.LoadFrom(file).GetTypes();
                }
                catch
                {
                    continue;  // Can't load as .NET assembly, so ignore
                }

                interestingTypes =
                    types.Where(t => t.IsClass &&
                                     t.GetInterfaces().Contains(typeof(IFixtureGenerator)));
            }
            return interestingTypes.ToList();
        }
    }
}