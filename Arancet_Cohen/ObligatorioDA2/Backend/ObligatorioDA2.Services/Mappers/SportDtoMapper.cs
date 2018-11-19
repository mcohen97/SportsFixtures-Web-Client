using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Services.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Mappers
{
    public class SportDtoMapper
    {
        public SportDto ToDto(Sport aSport) {
            return new SportDto() { name = aSport.Name, isTwoTeams = aSport.IsTwoTeams };
        }

    }
}
