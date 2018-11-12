using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.DataAccess;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Match = ObligatorioDA2.BusinessLogic.Match;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.BusinessLogic.FixtureAlgorithms;
using ObligatorioDA2.Services.Interfaces.Dtos;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Mappers;
using System.Diagnostics.CodeAnalysis;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FixtureServiceTest
    {
        private IFixtureGenerator oneMatchGenerator;
        private IFixtureGenerator twoMatchsGenerator;
        private EncounterDtoMapper mapper;
        private Sport sport;
        private Team teamA;
        private Team teamB;
        private Team teamC;
        private Team teamD;
        private ICollection<Team> teamsCollection;
        private DateTime initialDate;
        private IMatchRepository matchStorage;
        private ITeamRepository teamStorage;
        private ISportRepository sportStorage;
        private FixtureService fixtureService;
        private DatabaseConnection context;
        private EncounterFactory factory;

        [TestInitialize]
        public void Initialize()
        {
            factory = new EncounterFactory();
            sport = new Sport("Soccer",true);
            teamA = new Mock<Team>(1, "teamA", "photo", sport).Object;
            teamB = new Mock<Team>(2, "teamB", "photo", sport).Object;
            teamC = new Mock<Team>(3, "teamC", "photo", sport).Object;
            teamD = new Mock<Team>(4, "teamD", "photo", sport).Object;
            teamsCollection = new List<Team> {
                teamA,
                teamB,
                teamC,
                teamD
            };       

            initialDate = DateTime.Now;
            oneMatchGenerator = new OneMatchFixture(initialDate, 2, 5);
            twoMatchsGenerator = new HomeAwayFixture(initialDate, 2, 5);
            SetUpRepository();
            mapper = new EncounterDtoMapper(teamStorage, matchStorage, sportStorage);
            context.Database.EnsureDeleted();
            Mock<IAuthenticationService> auth = new Mock<IAuthenticationService>();
            MatchService service = new MatchService(matchStorage, teamStorage, sportStorage, auth.Object);
            fixtureService = new FixtureService(teamStorage, sportStorage,service,service, matchStorage, auth.Object);
        }

        private void SetUpRepository()
        {
            DbContextOptions<DatabaseConnection> options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: "FixtureService")
                .Options;
            context = new DatabaseConnection(options);
            matchStorage = new MatchRepository(context);
            teamStorage = new TeamRepository(context);
            sportStorage = new SportRepository(context);
        }

        private void AddTeamsToRepo()
        {
            foreach (Team team in teamsCollection)
            {
                teamStorage.Add(team);
            }
        }

        [TestMethod]
        public void SetFixtureAlgorithmTest()
        {
            fixtureService.FixtureAlgorithm = twoMatchsGenerator;
            Assert.AreEqual(fixtureService.FixtureAlgorithm, twoMatchsGenerator);
        }

        [TestMethod]
        public void AddFixtureOneMatchTest()
        {
            AddSportAndTeams();
            fixtureService.FixtureAlgorithm = oneMatchGenerator;
            ICollection<EncounterDto> matchesAdded = fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.id)));
        }

        [TestMethod]
        public void AddFixtureTwoMatchesTest()
        {
            AddSportAndTeams();
            fixtureService.FixtureAlgorithm = twoMatchsGenerator;
            ICollection<EncounterDto> matchesAdded = fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.id)));
        }

        [TestMethod]
        public void AddFixtureOneMatchImpairTest()
        {
            AddSportAndTeams();
            teamsCollection.Remove(teamD);
            fixtureService.FixtureAlgorithm = oneMatchGenerator;
            ICollection<EncounterDto> matchesAdded = fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.id)));
        }

        [TestMethod]
        public void AddFixtureTwoMatchesImpairTest()
        {
            AddSportAndTeams();
            teamsCollection.Remove(teamD);
            fixtureService.FixtureAlgorithm = twoMatchsGenerator;
            ICollection<EncounterDto> matchesAdded = fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.id)));
        }

        [TestMethod]
        public void AddFixtureWithNamesTest()
        {
            AddTeamsToRepo();
            fixtureService.FixtureAlgorithm = new OneMatchFixture(DateTime.Now, 2, 5);
            ICollection<string> teamsNames = teamsCollection.Select(t => t.Name).ToList();
            string sportName = sport.Name;
            ICollection<EncounterDto> matchesAdded = fixtureService.AddFixture(teamsNames, sportName);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.id)));
        }

        [TestMethod]
        public void AddFixtureOfSport()
        {
            AddTeamsToRepo();
            fixtureService.FixtureAlgorithm= new OneMatchFixture(DateTime.Now, 2, 5);
            ICollection<EncounterDto> matchesAdded = fixtureService.AddFixture(sport.Name);
            Assert.IsTrue(matchesAdded.All(m => matchStorage.Exists(m.id)));
        }

        //Exceptions

        [TestMethod]
        [ExpectedException(typeof(WrongFixtureException))]
        public void AddFixtureTeamAlreadyHasMatchTest()
        {
            AddSportAndTeams();
            fixtureService.FixtureAlgorithm = new OneMatchFixture(DateTime.Now, 2, 5);
            Encounter aMatch = factory.CreateEncounter(new List<Team>() { teamA, teamB }, initialDate, sport);
            matchStorage.Add(aMatch);
            fixtureService.AddFixture(teamsCollection);
            Assert.IsTrue(matchStorage.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(TeamNotFoundException))]
        public void AddFixtureTeamNotFoundTest()
        {
            ICollection<string> teamsNames = teamsCollection.Select(t => t.Name).ToList();
            string sportName = sport.Name;
            ICollection<EncounterDto> matchesAdded = fixtureService.AddFixture(teamsNames, sportName);
        }

        [TestMethod]
        public void GetAvailableStrategiesTest() {
            string path = "..//..//..//..//ObligatorioDA2.BusinessLogic//bin//Debug//";
            ICollection<Type> algorithms = fixtureService.GetAlgorithms(path);
            Assert.AreEqual(0, algorithms.Count);
        }

        private void AddSportAndTeams() {
            sportStorage.Add(sport);
            teamStorage.Add(teamA);
            teamStorage.Add(teamB);
            teamStorage.Add(teamC);
            teamStorage.Add(teamD);
        }
    }
}
