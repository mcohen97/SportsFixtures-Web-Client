using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
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
