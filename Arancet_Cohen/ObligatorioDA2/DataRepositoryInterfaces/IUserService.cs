﻿using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;

namespace ObligatorioDA2.Services.Interfaces
{
    public interface IUserService
    {
        User GetUser(string username);
        void AddUser(User testUser);
        void ModifyUser(User testUser);
        void DeleteUser(string userName);
        void FollowTeam(string username, Team toFollow);
        void FollowTeam(string userName, int idTeam);
        ICollection<Team> GetUserTeams(string userName);
        void UnFollowTeam(string userName, Team fake);
        void UnFollowTeam(string userName, int id);
        ICollection<User> GetAllUsers();
    }
}
