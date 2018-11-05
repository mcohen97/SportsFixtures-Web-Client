using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface ITeamService
    {
        Team GetTeam(int id);
        Team AddTeam(TeamDto testDto);
        Team Modify(TeamDto testDto);
        ICollection<Team> GetAllTeams();
        void DeleteTeam(int id);
        ICollection<Team> GetSportTeams(string name);
    }
}
