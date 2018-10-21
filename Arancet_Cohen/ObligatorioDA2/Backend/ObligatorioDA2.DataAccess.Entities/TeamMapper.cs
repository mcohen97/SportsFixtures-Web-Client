using System;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Entities;

namespace ObligatorioDA2.Data.DomainMappers
{
    public class TeamMapper
    {
        private SportMapper sportConverter;
        public TeamMapper()
        {
            sportConverter = new SportMapper();
        }
        public Team ToTeam(TeamEntity teamEntity)
        {

            Team convertedTeam = new Team(teamEntity.TeamNumber, teamEntity.Name, teamEntity.Photo, new Sport(teamEntity.SportEntityName));
            return convertedTeam;
        }

        public TeamEntity ToEntity(Team team)
        {
            SportEntity convertedSport = sportConverter.ToEntity(team.Sport);
            TeamEntity convertedTeam = new TeamEntity(team.Id, team.Name, team.Photo);
            convertedTeam.Sport = convertedSport;
            convertedTeam.SportEntityName = team.Sport.Name;
            return convertedTeam;
        }
    }
}