using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using ObligatorioDA2.DataAccess.Entities;

namespace ObligatorioDA2.DataAccess.Domain.Mappers
{
    public class MatchMapper
    {
        private TeamMapper teamConverter;
        private CommentMapper commentConverter;
        private SportMapper sportConverter;

        public MatchMapper()
        {
            teamConverter = new TeamMapper();
            commentConverter = new CommentMapper();
            sportConverter = new SportMapper();
        }
        public MatchEntity ToEntity( Match aMatch)
        {
            SportEntity sportEntity =sportConverter.ToEntity(aMatch.Sport);
            MatchEntity conversion = new MatchEntity()
            {
                Id = aMatch.Id,
                HomeTeam = teamConverter.ToEntity(aMatch.HomeTeam,aMatch.Sport.Name),
                AwayTeam = teamConverter.ToEntity(aMatch.AwayTeam,aMatch.Sport.Name),
                Date = aMatch.Date,
                Commentaries = TransformCommentaries(aMatch.GetAllCommentaries()),
                SportEntity = sportEntity
               
            };
            return conversion;
        }


        private ICollection<CommentEntity> TransformCommentaries(ICollection<Commentary> commentaries)
        {
            return commentaries.Select(c => commentConverter.ToEntity(c)).ToList();
        }

        public Match ToMatch(MatchEntity entity)
        {
            Team home = teamConverter.ToTeam(entity.HomeTeam);
            Team away = teamConverter.ToTeam(entity.AwayTeam);
            ICollection<Commentary> comments = entity.Commentaries.Select(ce => commentConverter.ToComment(ce)).ToList();
            DateTime date = entity.Date;
            Sport sport = sportConverter.ToSport(entity.SportEntity);
            Match created = new Match(entity.Id, home, away, date,sport,comments);
            return created;
        }
    }
}

