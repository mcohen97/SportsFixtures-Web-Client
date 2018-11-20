using System.Collections.Generic;
using ObligatorioDA2.Services.Contracts.Dtos;

namespace ObligatorioDA2.Services.Contracts
{
    public interface IUserService
    {
        UserDto GetUser(string username);
        UserDto AddUser(UserDto testUser);
        UserDto ModifyUser(UserDto testUser);
        void DeleteUser(string userName);
        void FollowTeam(string userName, int idTeam);
        ICollection<TeamDto> GetUserTeams(string userName);
        void UnFollowTeam(string userName, int id);
        ICollection<UserDto> GetAllUsers();
    }
}
