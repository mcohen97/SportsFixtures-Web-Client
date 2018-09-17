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

        public User ToUser(UserEntity toConvert)
        {
            User converted;
            if (toConvert.IsAdmin)
            {
                converted = new Admin(toConvert.Name, toConvert.Surname, toConvert.UserName, 
                    toConvert.Password, toConvert.Email,toConvert.Id);
            }
            else
            {
                converted = new Follower(toConvert.Name, toConvert.Surname, toConvert.UserName,
                    toConvert.Password, toConvert.Email, toConvert.Id);
            }
            return converted;
        }
    }
}
