using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Interfaces.Dtos
{
    public class ResultDto
    {
        public ICollection<Tuple<int, int>> teams_positions;
    }
}
