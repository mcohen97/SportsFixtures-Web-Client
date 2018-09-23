namespace ObligatorioDA2.DataAccess.Entities
{
    public class TeamEntity
    {
        public TeamEntity(){

        }

        public TeamEntity(int id, string name, string photo){
            Id = id;
            Name = name;
            Photo = photo;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }
    }
}