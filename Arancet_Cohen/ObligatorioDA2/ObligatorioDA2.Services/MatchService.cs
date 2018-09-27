using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
