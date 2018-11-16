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
using ObligatorioDA2.Services.Mappers;
using ObligatorioDA2.Services.Interfaces.Dtos;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;

namespace ObligatorioDA2.Services
{
    public class FixtureService : IFixtureService
    {
        private IInnerMatchService matchService;
        private ITeamRepository teamStorage;
        private IFixtureGenerator fixtureAlgorithm;
        private IAuthenticationService authenticator;
        private EncounterDtoMapper mapper;
        private const string DLL_EXTENSION = "*.dll";


        public FixtureService( ITeamRepository teamRepository,IInnerMatchService matchAddition,
            IAuthenticationService authService)
        {
            matchService = matchAddition;
            teamStorage = teamRepository;
            authenticator = authService;
            mapper = new EncounterDtoMapper();
        }

        public IFixtureGenerator FixtureAlgorithm { get { return fixtureAlgorithm; } set { SetFixtureAlgorithm(value); } }

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

        public ICollection<EncounterDto> AddFixture(ICollection<string> teamsNames, string sportName)
        {
            authenticator.AuthenticateAdmin();
            ICollection<Team> teamsCollection = teamsNames.Select(name => teamStorage.Get(sportName, name)).ToList();
            return AddFixture(teamsCollection);
        }

        public ICollection<EncounterDto> AddFixture(string sportName)
        {
            authenticator.AuthenticateAdmin();
            ICollection<Team> teamsCollection;
            try
            {
                teamsCollection = teamStorage.GetTeams(sportName);
            }
            catch (SportNotFoundException e) {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            return AddFixture(teamsCollection);
        }

        private ICollection<EncounterDto> AddFixture(ICollection<Team> teamsCollection)
        {
            authenticator.AuthenticateAdmin();
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

            return added.Select(e => mapper.ToDto(e)).ToList();

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
            authenticator.AuthenticateAdmin();
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