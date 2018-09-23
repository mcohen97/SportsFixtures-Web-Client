using System;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class TeamMapper
    {
        public Team ToTeam(TeamEntity teamEntity)
        {
            Team convertedTeam = new Team(teamEntity.Id, teamEntity.Name, teamEntity.Photo);
            return convertedTeam;
        }

        public TeamEntity ToEntity(Team team)
        {
            TeamEntity convertedTeam = new TeamEntity(team.Name, team.Photo);
            return convertedTeam;
        }
    }
}