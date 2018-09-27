namespace ObligatorioDA2.DataAccess.Entities
{
    public class TeamEntity:BaseEntity
    {
        public TeamEntity(){

        }

        public TeamEntity(int id, string name, string photo){
            Id = id;
            Name = name;
            Photo = photo;
        }

        public string Name { get; set; }

        public string Photo { get; set; }

        public int SportEntityId { get; set; }
    }
}