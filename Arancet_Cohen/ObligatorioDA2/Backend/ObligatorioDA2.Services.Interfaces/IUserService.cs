using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Services.Interfaces.Dtos;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IUserService
    {
        User GetUser(string username);
        void AddUser(UserDto testUser);
        void ModifyUser(UserDto testUser);
        void DeleteUser(string userName);
        void FollowTeam(string username, Team toFollow);
        void FollowTeam(string userName, int idTeam);
        ICollection<Team> GetUserTeams(string userName);
        void UnFollowTeam(string userName, Team fake);
        void UnFollowTeam(string userName, int id);
        ICollection<User> GetAllUsers();
    }
}
