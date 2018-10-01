using System;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class TeamMapper
    {
        private SportMapper sportConverter;
        public TeamMapper() {
            sportConverter = new SportMapper();
        }
        public Team ToTeam(TeamEntity teamEntity)
        {

            Team convertedTeam = new Team(teamEntity.Identity,teamEntity.Name, teamEntity.Photo, new Sport(teamEntity.SportEntityName));
            return convertedTeam;
        }

        public TeamEntity ToEntity(Team team)
        {
            SportEntity convertedSport = sportConverter.ToEntity(team.Sport);
            TeamEntity convertedTeam = new TeamEntity(team.Id, team.Name, team.Photo);
            convertedTeam.Sport = convertedSport;
            convertedTeam.SportEntityName = convertedSport.Name;
            return convertedTeam;
        }
    }
}