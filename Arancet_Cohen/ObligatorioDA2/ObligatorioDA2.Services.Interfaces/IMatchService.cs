using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IMatchService
    {
        Match AddMatch(Match aMatch);

        Match AddMatch(int homeTeamId, int awayTeamId, string sportName, DateTime date);

        Match AddMatch(int assignedId,int homeTeamId, int awayTeamId, string sportName, DateTime date);

        ICollection<Match> GetAllMatches();

        Match GetMatch(int anId);

        void DeleteMatch(int anId);

        void ModifyMatch(Match aMatch);

        void ModifyMatch(int idMatch, int idHome, int idAway, DateTime date, string sportName);

        ICollection<Match> GetAllMatches(string sportName);

        ICollection<Match> GetAllMatches(int teamId);

        bool Exists(int id);

        Commentary CommentOnMatch(int matchId, string userName, string text);

        ICollection<Commentary> GetMatchCommentaries(int matchId);
        ICollection<Commentary> GetAllCommentaries();
        Commentary GetComment(int id);
    }
}
