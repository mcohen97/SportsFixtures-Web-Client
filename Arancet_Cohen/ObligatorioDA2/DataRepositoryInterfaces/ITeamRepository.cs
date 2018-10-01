using BusinessLogic;
using System.Collections.Generic;

namespace DataRepositoryInterfaces
{
    public interface ITeamRepository
    {
        bool IsEmpty();

        void Clear();

        Team Get(string sportName, string teamName);

        void Delete(string sportName, string teamName);

        bool Exists(string sportName, string teamName);

        void Add( Team aTeam);

        void Modify( Team aTeam);

        ICollection<Team> GetAll();

        ICollection<Team> GetFollowedTeams(string username);
    }

}