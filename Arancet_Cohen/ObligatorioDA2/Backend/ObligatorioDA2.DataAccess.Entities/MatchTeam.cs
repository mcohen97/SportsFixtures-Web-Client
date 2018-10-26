using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Data.Entities
{
    public class MatchTeam
    {
        public TeamEntity Team { get; set; }
        public int TeamNumber { get; set; }
        public MatchEntity Match { get; set; }
        public int MatchId { get; set; }
        public int Position { get; set; }
    }
}
