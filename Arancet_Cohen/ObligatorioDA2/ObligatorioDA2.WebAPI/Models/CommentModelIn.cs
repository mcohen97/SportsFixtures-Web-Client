using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioDA2.WebAPI.Models
{
    public class CommentModelIn
    {
        [Required]
        public string Text { get; set; }
    }
}
