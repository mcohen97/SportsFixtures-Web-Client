using ObligatorioDA2.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Data.Repositories.Interfaces
{
    public interface ISportTableService
    {
        ICollection<Tuple<Team,int>> GetScoreTable(string sportName);
    }
}
