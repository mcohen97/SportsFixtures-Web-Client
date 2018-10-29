using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioDA2.WebAPI.Models
{
    public class ResultModel
    {
        public ICollection<Tuple<int, int>> Team_Position {get;set;}
    }
}
