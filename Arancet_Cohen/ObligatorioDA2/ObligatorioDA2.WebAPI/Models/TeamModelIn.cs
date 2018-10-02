using System.ComponentModel.DataAnnotations;


namespace ObligatorioDA2.WebAPI.Models
{
    public class TeamModelIn
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }  

        [Required]
        public string SportName { get; set; }

        public string Photo{ get; set; }
    }
}