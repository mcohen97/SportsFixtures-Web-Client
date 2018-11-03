using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Interfaces;

namespace ObligatorioDA2.Services
{
    public class TeamService: ITeamService
    {
        private ISportRepository sports;
        private ITeamRepository teams;
        private IAuthenticationService authentication;

        public TeamService(ISportRepository sportsRepo, ITeamRepository teamsRepo, IAuthenticationService authService)
        {
            sports = sportsRepo;
            teams = teamsRepo;
            authentication = authService;
        }
    }
}
