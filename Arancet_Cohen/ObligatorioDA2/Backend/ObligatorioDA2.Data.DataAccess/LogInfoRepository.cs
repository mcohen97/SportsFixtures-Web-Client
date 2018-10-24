using ObligatorioDA2.Data.Repositories.Interfaces;

namespace ObligatorioDA2.Data.DataAccess
{
    public class LogInfoRepository : ILogInfoRepository
    {
        private DatabaseConnection context;

        public LogInfoRepository(DatabaseConnection context)
        {
            this.context = context;
        }
    }
}