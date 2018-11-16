using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioDA2.WebAPI.Models
{
    public class EncounterCalendarModelOut
    {
        public string SportName { get; set; }
        public ICollection<EncountersGroupedByDate> EncountersByDate { get; set; }
    }
}
