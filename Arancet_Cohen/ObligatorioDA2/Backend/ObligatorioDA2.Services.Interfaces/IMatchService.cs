using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IMatchService
    {
        EncounterDto AddMatch(EncounterDto aMatch);

        EncounterDto AddMatch(ICollection<int> teamsIds, string sportName, DateTime date);

        EncounterDto AddMatch(int assignedId, ICollection<int> teamsIds, string sportName, DateTime date);

        ICollection<EncounterDto> GetAllMatches();

        EncounterDto GetMatch(int anId);

        void DeleteMatch(int anId);

        void ModifyMatch(EncounterDto aMatch);

        void ModifyMatch(int idMatch, ICollection<int> teamsIds, DateTime date, string sportName);

        ICollection<EncounterDto> GetAllEncounterDtos(string sportName);

        ICollection<EncounterDto> GetAllEncounterDtos(int teamId);

        bool Exists(int id);

        CommentaryDto CommentOnMatch(int matchId, string userName, string text);

        ICollection<CommentaryDto> GetMatchCommentaries(int matchId);
        ICollection<CommentaryDto> GetAllCommentaries();
        CommentaryDto GetComment(int id);
        void SetResult(int id, ResultDto resultDto);
    }
}
