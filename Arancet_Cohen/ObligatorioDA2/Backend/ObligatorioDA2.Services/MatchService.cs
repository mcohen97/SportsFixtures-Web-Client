using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Repositories.Interfaces;
using System.Linq;
using ObligatorioDA2.Services.Exceptions;
using ObligatorioDA2.Services.Interfaces;
using ObligatorioDA2.BusinessLogic.Data.Exceptions;
using ObligatorioDA2.Services.Interfaces.Dtos;
using ObligatorioDA2.Services.Mappers;
using ObligatorioDA2.BusinessLogic.Exceptions;

namespace ObligatorioDA2.Services
{
    public class MatchService : IInnerMatchService, IMatchService
    {
        private IMatchRepository matchesStorage;
        private ITeamRepository teamsStorage;
        private ISportRepository sportsStorage;
        private IUserRepository usersStorage;
        private EncounterFactory factory;
        private EncounterDtoMapper encounterConverter;
        private CommentaryDtoMapper commentConverter;

        public MatchService(IMatchRepository matchesRepository, ITeamRepository teamsRepository, ISportRepository sportsRepository)
        {
            factory = new EncounterFactory();
            matchesStorage = matchesRepository;
            teamsStorage = teamsRepository;
            sportsStorage = sportsRepository;
            encounterConverter = new EncounterDtoMapper(teamsStorage, matchesStorage,sportsStorage);
        }

        public MatchService(IMatchRepository matchsRepository, ITeamRepository teamsRepository, ISportRepository sportsRepository, IUserRepository usersRepository)
            : this(matchsRepository, teamsRepository, sportsRepository)
        {
            usersStorage = usersRepository;
            commentConverter = new CommentaryDtoMapper(usersStorage);

        }

        public EncounterDto AddMatch(ICollection<int> teamsIds, string sportName, DateTime date)
        {
            return AddMatch(0, teamsIds, sportName, date);
        }

        public EncounterDto AddMatch(int idMatch, ICollection<int> teamsIds, string sportName, DateTime encounterDate)
        {
            EncounterDto toAdd = new EncounterDto() { id = idMatch,sportName = sportName , date = encounterDate, teamsIds = teamsIds };
            return AddMatch(toAdd);
        }

        public EncounterDto AddMatch(EncounterDto anEncounter)
        {
            Encounter toAdd = TryCreateEncounter(anEncounter);
            Encounter added = AddMatch(toAdd);
            return encounterConverter.ToDto(added);
        }

        private Encounter TryCreateEncounter(EncounterDto anEncounter) {
            try
            {
                return encounterConverter.ToEncounter(anEncounter);
            }
            catch (InvalidMatchDataException e) {
                throw new ServiceException(e.Message, ErrorType.INVALID_DATA);
            }
        }

        public Encounter AddMatch(Encounter toAdd) {
            ValidateDate(toAdd);
            try
            {
                return matchesStorage.Add(toAdd);
            }
            catch (MatchAlreadyExistsException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_ALREADY_EXISTS);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public void ModifyMatch(int idMatch, ICollection<int> teamsIds, DateTime date, string sportName)
        {
            EncounterDto toModify = new EncounterDto() { id = idMatch, sportName = sportName, date = date, teamsIds = teamsIds };
            ModifyMatch(toModify);
        }

        public void ModifyMatch(EncounterDto anEncounter)
        {
            Encounter toAdd = encounterConverter.ToEncounter(anEncounter);
            ValidateDate(toAdd);
            try
            {
                matchesStorage.Modify(toAdd);
            }
            catch (MatchNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        private void ValidateDate(Encounter aMatch) {
            if (aMatch.GetParticipants().Any(t => DateOccupied(aMatch.Id, t, aMatch.Date)))
            {
                Team occupied = aMatch.GetParticipants().First(t => DateOccupied(aMatch.Id, t, aMatch.Date));
                throw new TeamAlreadyHasMatchException(occupied.Name + " already has a match on date " + aMatch.Date);
            }
        }

        private bool DateOccupied(int matchId, Team team, DateTime date)
        {
            if (matchId > 0)
            {
                return matchesStorage.GetAll().Any(m => (m.Id != matchId) && (m.GetParticipants().Any(t => t.Equals(team))) && SameDates(m.Date, date));
            }
            else
            {
                return matchesStorage.GetAll().Any(m => (m.GetParticipants().Any(t => t.Equals(team))) && SameDates(m.Date, date));
            }
        }

        private bool SameDates(DateTime date1, DateTime date2)
        {
            bool sameYear = date1.Year == date2.Year;
            bool sameMonth = date1.Month == date2.Month;
            bool sameDay = date1.Day == date2.Day;
            return sameYear && sameMonth && sameDay;
        }

        public ICollection<EncounterDto> GetAllMatches()
        {
            try
            {
                return matchesStorage.GetAll()
                    .Select(e => encounterConverter.ToDto(e))
                    .ToList();
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public EncounterDto GetMatch(int anId)
        {
            try
            {
                Encounter queried = matchesStorage.Get(anId);
                return encounterConverter.ToDto(queried);
            }
            catch (MatchNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public void DeleteMatch(int anId)
        {
            try
            {
                matchesStorage.Delete(anId);
            }
            catch (MatchNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e)
            {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public ICollection<EncounterDto> GetAllEncounterDtos(string sportName)
        {
            try
            {
                return GetAllMatches(sportName).Select(e => encounterConverter.ToDto(e)).ToList();
            }
            catch (SportNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
       }

        public ICollection<Encounter> GetAllMatches(string sportName) {
            if (!sportsStorage.Exists(sportName))
            {
                throw new ServiceException("Sport not found", ErrorType.ENTITY_NOT_FOUND);
            }
            return matchesStorage.GetAll().Where(m => m.Sport.Name.Equals(sportName)).ToList();
        }

        public ICollection<EncounterDto> GetAllEncounterDtos(int idTeam)
        {
            if (!teamsStorage.Exists(idTeam))
            {
                throw new ServiceException("Team not found", ErrorType.ENTITY_NOT_FOUND);
            }
            return matchesStorage.GetAll()
                .Where(m => m.GetParticipants()
                .Any(t => t.Id == idTeam))
                .Select(e => encounterConverter.ToDto(e))
                .ToList();
        }

        public bool Exists(int id)
        {
            try
            {
                return matchesStorage.Exists(id);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }     


        public CommentaryDto CommentOnMatch(int matchId, string userName, string commentText)
        {
            CommentaryDto dto = new CommentaryDto() { makerUsername = userName, text = commentText };
            Commentary toAdd = commentConverter.ToCommentary(dto);
            Commentary added = AddComment(matchId, toAdd); 
            dto.commentId = added.Id;
            return dto;
        }

        private Commentary AddComment(int matchId, Commentary toAdd)
        {
            try
            {
                return matchesStorage.CommentOnMatch(matchId, toAdd);
            }
            catch (MatchNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public ICollection<CommentaryDto> GetMatchCommentaries(int matchId)
        {
            try
            {
                Encounter stored = matchesStorage.Get(matchId);
                return stored.GetAllCommentaries()
                    .Select(c => commentConverter.ToDto(c))
                    .ToList();
            }
            catch (MatchNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public ICollection<CommentaryDto> GetAllCommentaries()
        {
            try
            {
                return matchesStorage.GetComments()
                    .Select(c => commentConverter.ToDto(c))
                    .ToList();
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public CommentaryDto GetComment(int id)
        {
            try
            {
                Commentary stored = matchesStorage.GetComment(id);
                return commentConverter.ToDto(stored);
            }
            catch (EntityNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);
            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);
            }
        }

        public void SetResult(int id, ResultDto resultDto)
        {
            try
            {
                TrySetResult(id, resultDto);
            }
            catch (EntityNotFoundException e)
            {
                throw new ServiceException(e.Message, ErrorType.ENTITY_NOT_FOUND);

            }
            catch (DataInaccessibleException e) {
                throw new ServiceException(e.Message, ErrorType.DATA_INACCESSIBLE);

            }

        }

        private void TrySetResult(int id, ResultDto resultDto)
        {
            Result result = LoadResult(resultDto.teams_positions);
            Encounter retrieved = matchesStorage.Get(id);
            try
            {
                retrieved.Result = result;
            }
            catch (InvalidMatchDataException e) {
                throw new ServiceException(e.Message, ErrorType.INVALID_DATA);
            }
            matchesStorage.Modify(retrieved);
        }

        private Result LoadResult(ICollection<Tuple<int, int>> teamsPositions)
        {
            Result loaded = new Result();
            Team current;
            foreach (Tuple<int, int> standing in teamsPositions) {
                current = teamsStorage.Get(standing.Item1);
                loaded.Add(current, standing.Item2);
            }
            return loaded;
        }
    }
}
