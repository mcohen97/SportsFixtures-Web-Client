
using System.ComponentModel.DataAnnotations;


namespace ObligatorioDA2.WebAPI.Models
{
    public class FixtureModelIn
    {
        [Required]
        public int Day { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }
    }
}
