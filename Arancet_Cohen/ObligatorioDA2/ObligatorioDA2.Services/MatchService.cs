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

        public MatchService(IMatchRepository aRepo)
        {
            matchesStorage = aRepo;
        }

        public void AddMatch(Match aMatch)
        {
            
        }

        public ICollection<Match> GetAllMatches()
        {
            throw new NotImplementedException();
        }

        public Match GetMatch(int anId)
        {
            throw new NotImplementedException();
        }

        public void DeleteMatch(int anId)
        {
            throw new NotImplementedException();
        }

        public void ModifyMatch(Match aMatch)
        {
            throw new NotImplementedException();
        }
    }
}
