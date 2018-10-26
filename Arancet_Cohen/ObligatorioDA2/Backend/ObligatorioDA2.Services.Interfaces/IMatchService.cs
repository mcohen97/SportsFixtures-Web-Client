using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IMatchService
    {
        Encounter AddMatch(Encounter aMatch);

        Encounter AddMatch(ICollection<int> teamsIds, string sportName, DateTime date);

        Encounter AddMatch(int assignedId, ICollection<int> teamsIds, string sportName, DateTime date);

        ICollection<Encounter> GetAllMatches();

        Encounter GetMatch(int anId);

        void DeleteMatch(int anId);

        void ModifyMatch(Encounter aMatch);

        void ModifyMatch(int idMatch, ICollection<int> teamsIds, DateTime date, string sportName);

        ICollection<Encounter> GetAllMatches(string sportName);

        ICollection<Encounter> GetAllMatches(int teamId);

        bool Exists(int id);

        Commentary CommentOnMatch(int matchId, string userName, string text);

        ICollection<Commentary> GetMatchCommentaries(int matchId);
        ICollection<Commentary> GetAllCommentaries();
        Commentary GetComment(int id);
    }
}
