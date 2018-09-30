﻿using System;
using System.Collections.Generic;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class UserEntity
    {
        public UserEntity()
        {
        }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public ICollection<UserTeam> FavouriteTeams{get;set;}
    }
}
