using BusinessLogic;

namespace DataRepositoryInterfaces
{
    public interface ITeamRepository
    {
         Team GetTeamByName(string name);
    }
}