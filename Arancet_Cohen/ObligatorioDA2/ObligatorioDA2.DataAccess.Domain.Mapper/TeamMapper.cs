using System;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class TeamMapper
    {
        public Team ToTeam(TeamEntity teamEntity)
        {
            Team convertedTeam = new Team(teamEntity.Identity,teamEntity.Name, teamEntity.Photo);
            return convertedTeam;
        }

        public TeamEntity ToEntity(Team team)
        {
            TeamEntity convertedTeam = new TeamEntity(team.Id, team.Name, team.Photo);
            convertedTeam.SportEntityName = team.Sport.Name;
            return convertedTeam;
        }
    }
}