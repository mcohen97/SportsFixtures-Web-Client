using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Data.Repositories.Interfaces;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObligatorioDA2.Services.Mappers
{
    public class ResultDtoMapper
    {
        private ITeamRepository teamsStorage;
        public ResultDtoMapper(ITeamRepository teamsRepo) {
            teamsStorage = teamsRepo;
        }

        public ResultDto ToDto(Result aResult) {
            ResultDto dto = new ResultDto()
            {
                teams_positions = aResult.GetPositions()
                .Select(p => new Tuple<int, int>(p.Item1.Id, p.Item2)).ToList()
            };
            return dto;
        }

        public Result ToResult(ResultDto dto) {
            Result conversion = new Result();
            foreach (Tuple<int, int> t in dto.teams_positions) {
                Team stored = TryGetTeam(t.Item1);
            }
            return conversion;
        }

        private Team TryGetTeam(int id)
        {
            try
            {
                return teamsStorage.Get(id);
            }
            catch (TeamNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }
    }
}
