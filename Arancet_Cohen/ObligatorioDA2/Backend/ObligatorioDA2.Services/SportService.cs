using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
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

        public ICollection<Sport> GetAllSports()
        {
            Authenticate();
            ICollection<Sport> allOfThem;
            try
            {
                allOfThem = sports.GetAll();
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return allOfThem;
        }

        private void AuthenticateAdmin()
        {
            if (!authenticator.IsLoggedIn())
            {
                throw new NotAuthenticatedException();
            }

            if (!authenticator.HasAdminPermissions())
            {
                throw new NoPermissionsException();
            }
        }

        private void Authenticate()
        {
            if (!authenticator.IsLoggedIn())
            {
                throw new NotAuthenticatedException();
            }
        }
    }
}
