using BusinessLogic;
using DataRepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class TeamService: ITeamService
    {
        ITeamRepository teams;
        public TeamService(ITeamRepository repo)
        {
            teams = repo;
        }

        public void AddTeam(Sport played, Team testTeam)
        {
            throw new NotImplementedException();
        }
    }
}
