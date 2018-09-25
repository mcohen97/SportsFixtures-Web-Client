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

        [TestInitialize]
        public void TestInitialize(){
            for(int i = 1; i <= 6; i++){
                Team newTeam = new Team(i, "Team "+1, "Photo/"+i);
                //newTeam.Setup(t => t.Equals(It.IsAny<object>())).Returns<object>(t => (t as Team)?.Id == i);
                teams.Add(newTeam);
            }
            initialDate = new DateTime(2019,1,1);
            finalDate = new DateTime(2019, 4,4);
            roundLength = 2;
            daysBetweenRounds = 5;
            oneMatchFixture = new OneMatchFixture(initialDate, finalDate, roundLength, daysBetweenRounds);
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
        public void GenerateOneMatchFixtureTenTeamsTest(){

            BusinessLogic.Match[] fixtureResult = oneMatchFixture.GenerateFixture(teams);
            Assert.AreEqual(15, fixtureResult.Length);
        }
    }
}