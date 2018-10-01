using BusinessLogic;
using System.Collections.Generic;
using RepositoryInterface;

namespace DataRepositoryInterfaces
{
    public interface ITeamRepository : IRepository<Team, int>
    {
        Team Get(string sportName, string teamName);

        bool Exists(string sportName, string teamName);

        void Delete(string sportName, string teamName);

        ICollection<Team> GetFollowedTeams(string username);

        ICollection<Team> GetTeams(string sportName);
    }

}