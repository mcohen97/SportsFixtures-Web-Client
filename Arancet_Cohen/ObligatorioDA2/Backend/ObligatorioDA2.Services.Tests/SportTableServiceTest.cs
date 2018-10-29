using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Interfaces;
using Match = ObligatorioDA2.BusinessLogic.Match;

namespace ObligatorioDA2.Services.Tests
{
    [TestClass]
    public class SportTableServiceTest
    {
        private SportTableService serviceToTest;
        private Mock<ISportRepository> sportsStorage;
        private Mock<ITeamRepository> teamsStorage;
        private Mock<IMatchService> matchesService;

        [TestInitialize]
        public void SetUp() {
            serviceToTest = new SportTableService();
            sportsStorage = new Mock<ISportRepository>();
            teamsStorage = new Mock<ITeamRepository>();
            matchesService= new Mock<IMatchService>();
            SetUpSports();
            SetUpTeams();
        }

        private void SetUpSports()
        {
            Sport twoTeamSport = new Sport("Soccer", true);
            Sport multipleTeamSport = new Sport("Archery", false);
            sportsStorage.Setup(r => r.Get("Soccer")).Returns(twoTeamSport);
            sportsStorage.Setup(r => r.Get("Archery")).Returns(multipleTeamSport);
            sportsStorage.Setup(r => r.Get(It.Is<string>(s => !s.Equals("Soccer") && !s.Equals("Archery"))))
                .Throws(new SportNotFoundException());
        }

        private void SetUpTeams()
        {
            Sport twoTeamSport = new Sport("Soccer", true);
            Team teamA = new Team(1,"teamA", "photoA", twoTeamSport);
            Team teamB = new Team(2,"teamA", "photoA", twoTeamSport);
            Team teamC = new Team(3,"teamA", "photoA", twoTeamSport);
            ICollection<Encounter> matches = CreateMatches(teamA, teamB, teamC,twoTeamSport);
            matchesService.Setup(r => r.GetAllMatches("Soccer")).Returns(matches);

            Sport multipleTeamSport = new Sport("Archery", false);
            Team athleteD = new Team(4,"teamD", "photoD", multipleTeamSport);
            Team athleteE = new Team(5,"teamE", "photoE", multipleTeamSport);
            Team athleteF = new Team(6,"teamF", "photoF", multipleTeamSport);
            ICollection<Encounter> competitions = CreateCompetitions(athleteD, athleteE,athleteF,multipleTeamSport);
            matchesService.Setup(r => r.GetAllMatches("Archery")).Returns(competitions);

            teamsStorage.Setup(r => r.GetTeams("Soccer")).Returns(new List<Team>() { teamA, teamB, teamC });
            teamsStorage.Setup(r => r.GetTeams("Archery")).Returns(new List<Team>() { athleteD, athleteE, athleteF });
        }

        private ICollection<Encounter> CreateMatches(Team teamA, Team teamB, Team teamC,Sport played)
        {
            //Create matches.
            Match matchAvsB = new Match(new List<Team>() {teamA,teamB },DateTime.Now,played);
            Match matchAvsC = new Match(new List<Team>() { teamA, teamC }, DateTime.Now, played);
            Match matchBvsC = new Match(new List<Team>() { teamB, teamC }, DateTime.Now, played);
            //Create results.
            Result resultAvsB = new Result();
            resultAvsB.Add(teamA, 1);
            resultAvsB.Add(teamB, 2);
            Result resultAvsC = new Result();
            resultAvsC.Add(teamA, 2);
            resultAvsC.Add(teamC, 1);
            Result resultBvsC = new Result();
            resultBvsC.Add(teamB, 1);
            resultBvsC.Add(teamC, 1);
            //Assign results to matches.
            matchAvsB.Result = resultAvsB;
            matchAvsC.Result = resultAvsC;
            matchBvsC.Result = resultBvsC;

            return new List<Encounter>() { matchAvsB, matchAvsC, matchBvsC };
        }
        private ICollection<Encounter> CreateCompetitions(Team teamD, Team teamE, Team teamF, Sport played)
        {
            //Create competition.
            Match competitiomDEF = new Match(new List<Team>() { teamD, teamE, teamF }, DateTime.Now, played);
            Match competitionED = new Match(new List<Team>() { teamE, teamD }, DateTime.Now, played);
            Match competitionEDF = new Match(new List<Team>() { teamE, teamD,teamF }, DateTime.Now, played);
            //Create results.
            Result resultDEF = new Result();
            resultDEF.Add(teamD, 2);
            resultDEF.Add(teamE, 3);
            resultDEF.Add(teamF, 1);
            Result resultED = new Result();
            resultED.Add(teamE, 2);
            resultED.Add(teamD, 1);
            Result resultEDF = new Result();
            resultEDF.Add(teamE, 1);
            resultEDF.Add(teamD, 2);
            resultEDF.Add(teamF, 3);
            //Assign results to competitions.
            competitiomDEF.Result = resultDEF;
            competitionED.Result = resultED;
            competitionEDF.Result = resultEDF;
            return new List<Encounter>() { competitiomDEF, competitionED, competitionEDF };
        }
    }
}
