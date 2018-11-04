using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface ISportService
    {
        ICollection<Sport> GetAllSports();
        Sport GetSport(string name);
        Sport AddSport(SportDto testDto);
        void DeleteSport(string name);
    }
}
