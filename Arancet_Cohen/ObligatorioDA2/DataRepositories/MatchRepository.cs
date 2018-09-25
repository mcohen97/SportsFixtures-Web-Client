using DataAccess;
using DataRepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositories
{
    public class MatchRepository : IMatchRepository
    {
        private DatabaseConnection context;

        public MatchRepository(DatabaseConnection context)
        {
            this.context = context;
        }
    }
}
