using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class OneMatchFixture : FixtureGenerator
    {
        private DateTime initialDate;
        private DateTime finalDate;
        private int roundLength;
        private int daysBetweenRounds;

        public OneMatchFixture(DateTime initialDate, DateTime finalDate, int roundLength, int daysBetweenRounds)
        {
            this.initialDate = initialDate;
            this.finalDate = finalDate;
            this.roundLength = roundLength;
            this.daysBetweenRounds = daysBetweenRounds;
        }

        public override DateTime InitialDate { get => initialDate; set => SetInitialDate(value); }
        public override DateTime FinalDate { get => finalDate; set => SetFinalDate(value); }
        public override int RoundLength { get => roundLength; set => SetRoundLength(value); }
        public override int DaysBetweenRounds { get => daysBetweenRounds; set => SetDaysBetweenRounds(value); }

        public override Match[] GenerateFixture(ICollection<Team> teams)
        {
             int matches;
             int matchesPerRound;
             int Rounds;
            return new Match[3];
        }

        private void SetInitialDate(DateTime value)
        {
            initialDate = value;
        }

        private void SetFinalDate(DateTime value)
        {
            finalDate = value;
        }

        private void SetRoundLength(int value)
        {
            roundLength = value;
        }

        private void SetDaysBetweenRounds(int value)
        {
            daysBetweenRounds = value;
        }
   
    }
}