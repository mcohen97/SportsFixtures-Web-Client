using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic
{
    public class EncounterFactory
    {
        public Encounter CreateEncounter(List<Team> list, DateTime date, Sport sport)
        {
            Encounter built;
            if (sport.IsTwoTeams)
            {
                built = new Match(list, date, sport);
            }
            else {
                built = new Competition(list, date, sport);
            }
            return built;
        }
    }
}
