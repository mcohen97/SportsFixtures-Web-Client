using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.DataAccess.Entities;
using BusinessLogic;
using BusinessLogic.Factories;
using System.Linq;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class UserMapper
    {
        private UserFactory factory;
        private TeamMapper teamConverter;

        public UserMapper() {
            factory = new UserFactory();
            teamConverter = new TeamMapper();
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
                //FavouriteTeams = toConvert.GetFavouriteTeams().Select(t => BuldTeamUser(t,toConvert)).ToList()
            };
            return converted;
        }

        /*public ICollection<UserTeam> GetTeams(User aUser) {
            UserEntity entity = ToEntity(aUser);
            ICollection<UserTeam> conversion = aUser.GetFavouriteTeams()
                .Select(t => BuildRelationship(entity,t))
                .ToList();
            return conversion;
        }*/

        private UserTeam BuildRelationship(UserEntity entity, Team aTeam, string sportName)
        {
            TeamEntity teamEntity = teamConverter.ToEntity(aTeam);
            UserTeam relationship = new UserTeam
            {
                Team = teamEntity,
                Follower = entity,
                TeamEntitySportEntityName = teamEntity.SportEntityName,
                TeamEntityName = teamEntity.Name,
                UserEntityUserName = entity.UserName
            };
            return relationship;
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
                converted = factory.CreateAdmin(identity);
            }
            else
            {
                converted = factory.CreateFollower(identity);
            }
            return converted;
        }

        public User ToUser(UserEntity toConvert, ICollection<TeamEntity> teamEntities) {
            UserId identity = new UserId()
            {
                Name = toConvert.Name,
                Surname = toConvert.Surname,
                UserName = toConvert.UserName,
                Password = toConvert.Password,
                Email = toConvert.Email
            };
            ICollection<Team> teams = teamEntities.Select(t => teamConverter.ToTeam(t)).ToList();
            User converted;
            if (toConvert.IsAdmin)
            {
                converted = factory.CreateAdmin(identity, teams);
            }
            else
            {
                converted = factory.CreateFollower(identity, teams);
            }
            return converted;
        }
    }
}
