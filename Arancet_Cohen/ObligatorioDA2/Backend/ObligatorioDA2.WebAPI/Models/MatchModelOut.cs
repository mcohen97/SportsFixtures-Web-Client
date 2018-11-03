using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioDA2.WebAPI.Models
{
    public class MatchModelOut:EncounterModelOut
    {
        public bool HasWinner { get; set; }
        public int WinnerId { get; set; }
    }
}
