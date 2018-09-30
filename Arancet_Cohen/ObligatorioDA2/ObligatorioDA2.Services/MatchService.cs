using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using DataRepositoryInterfaces;

namespace ObligatorioDA2.Services
{
    public class MatchService
    {
        private IMatchRepository matchesStorage;
        private ITeamRepository teamsStorage;

        public MatchService(IMatchRepository matchsRepository, ITeamRepository teamsRepository)
        {
            matchesStorage = matchsRepository;
            teamsStorage = teamsRepository;
        }

        public void AddMatch(Match aMatch)
        {
            matchesStorage.Add(aMatch);
        }

        public ICollection<Match> GetAllMatches()
        {
            return matchesStorage.GetAll();
        }

        public Match GetMatch(int anId)
        {
            return matchesStorage.Get(anId);
        }

        public void DeleteMatch(int anId)
        {
            matchesStorage.Delete(anId);
        }

        public void ModifyMatch(Match aMatch)
        {
            matchesStorage.Modify(aMatch);
        }
    }
}
