using System;
using System.Collections.Generic;

namespace ObligatorioDA2.Services.Interfaces.Dtos
{
    public class ResultDto
    {
        public ICollection<Tuple<int, int>> teams_positions;
    }
}
