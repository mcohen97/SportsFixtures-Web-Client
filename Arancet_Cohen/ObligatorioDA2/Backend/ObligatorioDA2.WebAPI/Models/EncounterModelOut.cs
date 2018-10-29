using System;
using System.Collections.Generic;

namespace ObligatorioDA2.WebAPI.Models
{
    public abstract class EncounterModelOut
    {
        public int Id { get; set; }
        public string SportName { get; set; }
        public ICollection<int> TeamsIds { get; set; } 
        public DateTime Date { get; set; }
        public ICollection<int> CommentsIds { get; set; }
        public bool HasResult { get; set; }
    }
}
