using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioDA2.WebAPI.Models
{
    public class CommentModelOut
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string MakerUsername { get; set; }
    }
}
