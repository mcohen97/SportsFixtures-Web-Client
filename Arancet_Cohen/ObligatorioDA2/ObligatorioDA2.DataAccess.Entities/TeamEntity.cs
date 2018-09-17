namespace ObligatorioDA2.DataAccess.Entities
{
    public class TeamEntity
    {
        public TeamEntity(){

        }

        public TeamEntity(string name, string photo){
            Name = name;
            Photo = photo;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }
    }
}