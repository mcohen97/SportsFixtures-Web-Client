using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface ITeamService
    {
        TeamDto GetTeam(int id);
        TeamDto AddTeam(TeamDto testDto);
        TeamDto Modify(TeamDto testDto);
        ICollection<TeamDto> GetAllTeams();
        void DeleteTeam(int id);
        ICollection<TeamDto> GetSportTeams(string name);
    }
}
