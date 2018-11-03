using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Exceptions;

namespace ObligatorioDA2.Services
{
    public class TeamService: ITeamService
    {
        private ISportRepository sports;
        private ITeamRepository teams;
        private IAuthenticationService authentication;

        public TeamService(ISportRepository sportsRepo, ITeamRepository teamsRepo, IAuthenticationService authService)
        {
            sports = sportsRepo;
            teams = teamsRepo;
            authentication = authService;
        }

        public Team GetTeam(int id)
        {
            Team toReturn;
            if (authentication.IsLoggedIn())
            {
                toReturn = TryGetTeam(id);
            }
            else {
                throw new NotAuthenticatedException();
            }
            return toReturn;
        }

        private Team TryGetTeam(int id)
        {
            Team toReturn;
            try
            {
                toReturn = teams.Get(id);
            }
            catch (TeamNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return toReturn;
        }
    }
}
