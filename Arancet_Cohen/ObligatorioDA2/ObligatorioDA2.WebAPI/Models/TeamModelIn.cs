using System.ComponentModel.DataAnnotations;


namespace ObligatorioDA2.WebAPI.Models
{
    public class TeamModelIn
    {
        [Required]
        public string Name { get; set; }  

        public string Photo{ get; set; }
    }
}