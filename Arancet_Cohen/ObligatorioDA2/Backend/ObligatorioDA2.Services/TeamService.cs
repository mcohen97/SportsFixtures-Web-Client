using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.BusinessLogic.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.Services.Interfaces.Dtos;

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

        public Team AddTeam(TeamDto team)
        {
            AuthenticateAdmin();
            return TryAddTeam(team);
        }

        private Team TryAddTeam(TeamDto team)
        {
            Sport sport = GetSport(team.sportName);
            Team toAdd = CreateTeam(team, sport);
            Team added;
            try
            {
                added = teams.Add(toAdd);
            }
            catch (TeamAlreadyExistsException e) {
                throw new ServiceException(e.Message, ErrorType.ENTITY_ALREADY_EXISTS);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return added;
        }

        private Sport GetSport(string sportName)
        {
            Sport stored;
            try {
                stored = sports.Get(sportName);
            }
            catch (SportNotFoundException e) {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            return stored;
        }

        private Team CreateTeam(TeamDto team, Sport sport)
        {
            Team created;
            try
            {
                created = new Team(team.name, team.photo, sport);
            }
            catch (InvalidTeamDataException e) {
                throw new ServiceException(e.Message, ErrorType.INVALID_DATA);
            }
            return created;
        }

        public Team Modify(TeamDto testDto)
        {
            AuthenticateAdmin();
            Team old = TryGetTeam(testDto.id);
            Team updated = Update(old, testDto);
            teams.Modify(updated);
            return updated;
        }

        private Team Update(Team old, TeamDto testDto)
        {
            int id = old.Id;
            string name = string.IsNullOrWhiteSpace(testDto.name) ? old.Name : testDto.name;
            string photoPath = string.IsNullOrWhiteSpace(testDto.photo) ? old.PhotoPath : testDto.photo;
            return new Team(id, name, photoPath, old.Sport);
        }

        public Team GetTeam(int id)
        {
            if (!authentication.IsLoggedIn())
            {
                throw new NotAuthenticatedException();
            }
            return TryGetTeam(id);
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

        public ICollection<Team> GetAllTeams()
        {
            Authenticate();
            ICollection<Team> allOfThem;
            try
            {
                allOfThem = teams.GetAll();
            }catch(DataInaccessibleException e)
            {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
            return allOfThem;
        }

        private void AuthenticateAdmin() {
            if (!authentication.IsLoggedIn())
            {
                throw new NotAuthenticatedException();
            }

            if (!authentication.HasAdminPermissions())
            {
                throw new NoPermissionsException();
            }
        }

        private void Authenticate() {
            if (!authentication.IsLoggedIn())
            {
                throw new NotAuthenticatedException();
            }
        }
    }
}
