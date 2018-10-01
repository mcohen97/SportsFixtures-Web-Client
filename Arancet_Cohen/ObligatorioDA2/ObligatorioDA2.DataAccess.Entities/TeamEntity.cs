using System.Collections.Generic;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class TeamEntity
    {
        public TeamEntity()
        {

        }

        public TeamEntity(int id, string name, string photo)
        {
            Identity = id;
            Name = name;
            Photo = photo;
        }

        public int Identity { get; set; }
        public string Name { get; set; }

        public string Photo { get; set; }

        public string SportEntityName { get; set; }
        public SportEntity Sport { get; set; }

    }
}