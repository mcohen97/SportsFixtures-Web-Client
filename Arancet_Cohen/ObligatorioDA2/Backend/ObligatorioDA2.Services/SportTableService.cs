using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Sport asked = sportsStorage.Get(sportName);
            ICollection<Team> sportTeams = teamsStorage.GetTeams(sportName);
            ICollection<Encounter> teamsEncounters = matchesService.GetAllMatches(sportName);
            ICollection<Tuple<Team, int>> positions = CalculateCompetitionsTable(sportTeams, teamsEncounters);          
            return positions;
        }

        private ICollection<Tuple<Team, int>> CalculateCompetitionsTable(ICollection<Team> sportTeams, ICollection<Encounter> teamsEncounters)
        {
            Dictionary<Team, int> table = sportTeams.ToDictionary(item => item, item => 0);
            foreach (Encounter game in teamsEncounters){
                List<Tuple<Team, int>> positions = game.Result.GetPositions().ToList();
                positions.Sort((x, y) => x.Item2.CompareTo(y.Item2));

                table[positions[0].Item1] += 3;
                table[positions[1].Item1] += 2;
                if (positions.Count > 2)
                {
                    table[positions[2].Item1] += 1;
                }
            }
            return table.Keys.Select(t => new Tuple<Team, int>(t, table[t])).ToList();
        }

    }
}
