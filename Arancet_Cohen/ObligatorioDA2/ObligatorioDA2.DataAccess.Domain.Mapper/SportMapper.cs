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
                Name= aSport.Name,
            };
            return converted;
        }

        public Sport ToSport(SportEntity entity)
        {
            Sport conversion = new Sport( entity.Name);
            return conversion;
        }
    }
}
