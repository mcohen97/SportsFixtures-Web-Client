namespace ObligatorioDA2.WebAPI.Models
{
    public class TeamModelOut
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public byte[] Photo{ get; set; }
        public string SportName { get; set; }
    }
}