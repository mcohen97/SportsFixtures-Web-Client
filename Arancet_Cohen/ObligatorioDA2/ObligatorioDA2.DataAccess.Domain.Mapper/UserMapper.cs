using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.DataAccess.Entities;
using BusinessLogic;
using BusinessLogic.Factories;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class UserMapper
    {
        UserFactory factory;

        public UserMapper() {
            factory = new UserFactory();
        }
       
        public UserEntity ToEntity(User toConvert)
        {
            UserEntity converted = new UserEntity
            {
                Name = toConvert.Name,
                Surname = toConvert.Surname,
                UserName = toConvert.UserName,
                Password = toConvert.Password,
                Email = toConvert.Email,
                IsAdmin = toConvert.IsAdmin,
                Id = toConvert.Id
            };
            return converted;
        }

        public User ToUser(UserEntity toConvert)
        {
            UserId identity = new UserId() {
               Name= toConvert.Name,
               Surname = toConvert.Surname,
               UserName = toConvert.UserName,
               Password= toConvert.Password,
               Email = toConvert.Email
            };
            User converted;
            if (toConvert.IsAdmin)
            {
                converted = factory.CreateAdmin(identity,toConvert.Id);
            }
            else
            {
                converted = factory.CreateFollower(identity, toConvert.Id);
            }
            return converted;
        }
    }
}
