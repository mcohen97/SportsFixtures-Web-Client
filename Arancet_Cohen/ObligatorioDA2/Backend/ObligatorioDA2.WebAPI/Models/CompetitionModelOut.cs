using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioDA2.WebAPI.Models
{
    public class CompetitionModelOut:EncounterModelOut
    {
        public ICollection<StandingModelOut> Team_Position { get; set; }
    }
}
