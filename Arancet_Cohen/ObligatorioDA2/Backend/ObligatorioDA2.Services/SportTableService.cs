using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class SportTableService : ISportTableService
    {
        private ISportRepository sportsStorage;
        private ITeamRepository teamsStorage;
        private IMatchService matchesService;

        public SportTableService(ISportRepository sportsRepo, ITeamRepository teamsRepo, IMatchService service)
        {
            sportsStorage = sportsRepo;
            teamsStorage = teamsRepo;
            matchesService = service;
        }

        public ICollection<Tuple<Team, int>> GetScoreTable(string sportName)
        {
            throw new NotImplementedException();
        }
    }
}
