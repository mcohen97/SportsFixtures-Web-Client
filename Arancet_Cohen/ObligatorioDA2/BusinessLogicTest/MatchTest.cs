using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLogic;
using System.Collections.Generic;
using Moq;
using BusinessLogic.Exceptions;

namespace BusinessLogicTest
{
    [TestClass]
    public class MatchTest
    {
        private Mock<Team> teamA;
        private Mock<Team> teamB;
        private Mock<Sport> sport;
        private DateTime date;
        private Match match;

        [TestInitialize]
        public void TestInitialize(){
            teamA = new Mock<Team>(1, "TeamA", "Photo/A");
            teamB = new Mock<Team>(2, "TeamB", "Photo/B");
            date =  new DateTime(2019,1,25,13,30,0);
            match = new Match(teamA.Object, teamB.Object, date);
        }


    }
}