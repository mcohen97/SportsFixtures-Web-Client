using System;
using System.Collections.Generic;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.BusinessLogic.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;

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

        public Sport GetSport(string name)
        {
            Authenticate();
            Sport fromStorage;
            try
            {
                fromStorage = sports.Get(name);
            }
            catch (SportNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return fromStorage;
        }

        public Sport AddSport(SportDto dto)
        {
            AuthenticateAdmin();
            Sport toAdd = TryCreate(dto);
            try {
                sports.Add(toAdd);
            }
            catch (SportAlreadyExistsException e) {
                throw new ServiceException(e.Message, ErrorType.ENTITY_ALREADY_EXISTS);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return toAdd;
        }

        private Sport TryCreate(SportDto dto)
        {
            Sport built;
            try {
                built = new Sport(dto.name, dto.isTwoTeams);
            }
            catch (InvalidSportDataException e) {
                throw new ServiceException(e.Message, ErrorType.INVALID_DATA);
            }
            return built;
        }

        public void DeleteSport(string name)
        {
            AuthenticateAdmin();
            try {
                sports.Delete(name);
            }
            catch (SportNotFoundException e) {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
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
