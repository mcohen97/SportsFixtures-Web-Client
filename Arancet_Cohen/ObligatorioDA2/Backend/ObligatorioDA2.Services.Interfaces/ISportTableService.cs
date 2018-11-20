using ObligatorioDA2.Services.Interfaces.Dtos;
using System;
using System.Collections.Generic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface ISportTableService
    {
        ICollection<Tuple<TeamDto,int>> GetScoreTable(string sportName);
    }
}
