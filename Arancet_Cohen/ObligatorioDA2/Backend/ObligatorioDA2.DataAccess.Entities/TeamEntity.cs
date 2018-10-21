using System.Collections.Generic;

namespace ObligatorioDA2.Data.Entities
{
    public class TeamEntity
    {
        public TeamEntity()
        {

        }

        public TeamEntity(int id, string name, string photo)
        {
            TeamNumber = id;
            Name = name;
            Photo = photo;
        }

        public int TeamNumber { get; set; }
        public string Name { get; set; }

        public string Photo { get; set; }

        public string SportEntityName { get; set; }
        public SportEntity Sport { get; set; }

    }
}