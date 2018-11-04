using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Interfaces;

namespace ObligatorioDA2.Services
{
    public class SportService : ISportService
    {
        private ISportRepository sports;
        private ITeamRepository teams;
        private IAuthenticationService authenticator;

        public SportService(ISportRepository sportRepository, ITeamRepository teamRepository, IAuthenticationService authService)
        {
            sports = sportRepository;
            teams = teamRepository;
            authenticator = authService;
        }
    }
}
