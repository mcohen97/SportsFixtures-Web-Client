using System.Collections.Generic;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface ISportService
    {
        ICollection<SportDto> GetAllSports();
        SportDto GetSport(string name);
        SportDto AddSport(SportDto testDto);
        void DeleteSport(string name);
    }
}
