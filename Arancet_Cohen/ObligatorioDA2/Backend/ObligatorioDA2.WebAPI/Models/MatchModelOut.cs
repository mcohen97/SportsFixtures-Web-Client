using System;
using System.Collections.Generic;

namespace ObligatorioDA2.WebAPI.Models
{
    public class MatchModelOut
    {
        public int Id { get; set; }
        public string SportName { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime Date { get; set; }
        public ICollection<int> CommentsIds { get; set; } 

    }
}
