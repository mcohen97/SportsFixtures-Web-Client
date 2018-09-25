using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class MatchMapper
    {
        private TeamMapper teamConverter;
        private CommentMapper commentConverter;

        public MatchMapper() {
            teamConverter = new TeamMapper();
            commentConverter = new CommentMapper();
        }
        public MatchEntity ToEntity(Match aMatch)
        {
            /* MatchEntity conversion = new MatchEntity() {
                 HomeTeam
             }*/
            return null;
        }
    }
}
