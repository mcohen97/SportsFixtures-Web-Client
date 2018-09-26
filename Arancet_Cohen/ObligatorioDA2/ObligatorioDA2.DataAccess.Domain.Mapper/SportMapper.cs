using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class SportMapper
    {
        private TeamMapper teamConverter;
        public SportMapper() {
            teamConverter = new TeamMapper();
        }
        public SportEntity ToEntity(Sport aSport)
        {
            SportEntity converted = new SportEntity()
            {
                Id=aSport.Id,
                Name= aSport.Name,
                Teams = aSport.GetTeams().Select(t => teamConverter.ToEntity(t)).ToList()
            };
            return converted;
        }

        public Sport ToSport(SportEntity entity)
        {
            ICollection<Team> teams = entity.Teams.Select(te => teamConverter.ToTeam(te)).ToList();
            Sport conversion = new Sport(entity.Id, entity.Name, teams);
            return conversion;
        }
    }
}
