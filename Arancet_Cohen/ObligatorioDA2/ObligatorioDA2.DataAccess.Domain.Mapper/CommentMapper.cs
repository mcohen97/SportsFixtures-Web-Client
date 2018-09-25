using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class CommentMapper
    {
        private UserMapper usersMapper;

        public CommentMapper() {
            usersMapper =new UserMapper();
        }
        public CommentEntity ToEntity(Commentary comment)
        {

            CommentEntity converted = new CommentEntity()
            {
                Id = comment.Id,
                Text = comment.Text,
                Maker = usersMapper.ToEntity(comment.Maker)
            };
            return converted;
        }

        public Commentary ToComment(CommentEntity commentEntity)
        {
            User maker = usersMapper.ToUser(commentEntity.Maker);
            Commentary conversion= new Commentary(commentEntity.Id,commentEntity.Text,maker );
            return conversion;
        }
    }
}
