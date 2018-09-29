using DataRepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class SportService:ISportService
    {
        private ISportRepository sports;

        public SportService(ISportRepository repo) {
            sports = repo;
        }
    }
}
