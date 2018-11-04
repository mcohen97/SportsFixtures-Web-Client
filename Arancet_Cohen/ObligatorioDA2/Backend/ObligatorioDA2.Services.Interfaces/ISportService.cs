using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface ISportService
    {
        ICollection<Sport> GetAllSports();
        Sport GetSport(string name);
    }
}
