using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioDA2.WebAPI.Models
{
    public class MatchModelOut
    {
        [Required]
        public string SportName { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        [Required]
        public int AwayTeamId { get; set; }

        public ICollection<int> CommentsIds { get; set; } 

    }
}
