using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.DataAccess.Entities;
using BusinessLogic;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class UserMapper
    {
        public UserEntity ToEntity(User toConvert)
        {
            UserEntity converted;
            if (toConvert.IsAdmin())
            {
                converted = new AdminEntity();
            }
            else {
                converted = new FollowerEntity();
            }
            converted.Name = toConvert.Name;
            converted.Surname = toConvert.Surname;
            converted.UserName = toConvert.UserName;
            converted.Password = toConvert.Password;
            converted.Email = toConvert.Email;
            return converted;
        }
    }
}
