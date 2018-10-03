using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositoryInterfaces
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

        ICollection<Match> GetAllMatches(Sport sport);

        ICollection<Match> GetAllMatches(Team team);

        bool Exists(int id);

    }
}
