using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using System.Collections.Generic;
using Moq;
using BusinessLogic.Exceptions;
using System.Linq;

namespace BusinessLogicTest
{
    [TestClass]
    public class FixtureTest
    {

        private ICollection<Team> teams;
        private FixtureGenerator oneMatchFixture;
        private DateTime initialDate;
        private DateTime finalDate;
        private int roundLength;
        private int daysBetweenRounds;
        private Mock<Sport> played;

        [TestInitialize]
        public void TestInitialize(){
            teams = new List<Team>();
            for(int i = 1; i <= 6; i++){
                Team newTeam = new Team(i, "Team "+i, "Photo/"+i);
                //newTeam.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == i);
                teams.Add(newTeam);
            }
            initialDate = new DateTime(2019,1,1);
            finalDate = new DateTime(2019, 4,4);
            roundLength = 2;
            daysBetweenRounds = 5;
            played = new Mock<Sport>("Soccer");
            oneMatchFixture = new OneMatchFixture(initialDate, finalDate, roundLength, daysBetweenRounds,played.Object);
        }

        [TestMethod]
        public void ConstructorTest(){
            Assert.IsNotNull(oneMatchFixture);
        }

        [TestMethod]
        public void SetInitialDateTest(){
            DateTime newInitialDate = new DateTime(2019,3,3);
            oneMatchFixture.InitialDate = newInitialDate;
            Assert.AreEqual(newInitialDate, oneMatchFixture.InitialDate);
        }

        [TestMethod]
        public void SetFinalDateTest(){
            DateTime newFinalDate = new DateTime(2019,3,3);
            oneMatchFixture.FinalDate = newFinalDate;
            Assert.AreEqual(newFinalDate, oneMatchFixture.FinalDate);
        }

        [TestMethod]
        public void SetRoundLengthTest(){
            int newMax = 3;
            oneMatchFixture.RoundLength = newMax;
            Assert.AreEqual(newMax, oneMatchFixture.RoundLength);
        }

        [TestMethod]
        public void SetDaysBetweenRoundsTest(){
            int newDays = 6;
            oneMatchFixture.DaysBetweenRounds = newDays;
            Assert.AreEqual(newDays, oneMatchFixture.DaysBetweenRounds);
        }

        [TestMethod]
        public void GenerateOneMatchFixture6TeamsTest(){

            ICollection<BusinessLogic.Match> fixtureResult = oneMatchFixture.GenerateFixture(teams);
            ICollection<Team> copy = new List<Team>(teams);
            bool everyMatch = true;
            foreach (BusinessLogic.Match actualMatch in GenereteMatches(copy))
            {
                everyMatch = everyMatch && CheckMatchInFixture(fixtureResult, actualMatch);
            }
            Assert.AreEqual(15, fixtureResult.Count);
            Assert.IsTrue(everyMatch);
        }

        [TestMethod]
        public void GenerateOneMatchFixture5TeamsTest(){
            teams.Remove(teams.Last());
            ICollection<Team> copy = new List<Team>(teams);
            ICollection<BusinessLogic.Match> fixtureResult = oneMatchFixture.GenerateFixture(teams);
            bool everyMatch = true;
            foreach (BusinessLogic.Match actualMatch in GenereteMatches(copy))
            {
                everyMatch = everyMatch && CheckMatchInFixture(fixtureResult, actualMatch);
            }
            Assert.AreEqual(10, fixtureResult.Count);
            Assert.IsTrue(everyMatch);
        }

        private ICollection<BusinessLogic.Match> GenereteMatches(ICollection<Team> teams){
            ICollection<BusinessLogic.Match> matchesGenerated = new List<BusinessLogic.Match>();
            Team[] teamsArray = teams.ToArray();
            for (int i = 0; i < teamsArray.Length; i++)
            {
                for (int j = i; j < teamsArray.Length; j++)
                {
                    if(i != j)
                        matchesGenerated.Add(new BusinessLogic.Match(teamsArray[i], teamsArray[j], new DateTime(),played.Object));
                }
            }
            return matchesGenerated;
        }

        private bool CheckMatchInFixture (ICollection<BusinessLogic.Match> fixture, BusinessLogic.Match match){
            return fixture.Any(m => 
                (m.HomeTeam.Equals(match.HomeTeam) || m.HomeTeam.Equals(match.AwayTeam)) &&
                (m.AwayTeam.Equals(match.AwayTeam) || m.AwayTeam.Equals(match.HomeTeam))             
            );
        }
    }
}