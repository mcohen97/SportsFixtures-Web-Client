using System;
using System.Collections.Generic;

namespace ObligatorioDA2.WebAPI.Models
{
    public class MatchModelOut
    {
        public int Id { get; set; }
        public string SportName { get; set; }
        public ICollection<int> TeamsIds { get; set; } 
        public DateTime Date { get; set; }
        public ICollection<int> CommentsIds { get; set; } 

    }
}
