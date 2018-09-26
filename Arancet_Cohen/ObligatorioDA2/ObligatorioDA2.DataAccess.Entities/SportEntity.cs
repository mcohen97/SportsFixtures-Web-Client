using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class SportEntity:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<TeamEntity> Teams { get; set; }
    }
}
