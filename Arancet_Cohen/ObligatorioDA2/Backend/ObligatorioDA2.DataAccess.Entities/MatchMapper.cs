using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Entities;

namespace ObligatorioDA2.Data.DomainMappers
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
        public MatchEntity ToEntity(Match aMatch)
        {
            SportEntity sportEntity = sportConverter.ToEntity(aMatch.Sport);
            MatchEntity conversion = new MatchEntity()
            {
                Id = aMatch.Id,
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

        public Match ToMatch(MatchEntity aMatch, ICollection<MatchTeam> teamEntities)
        {
            ICollection<Commentary> comments = aMatch.Commentaries.Select(ce => commentConverter.ToComment(ce)).ToList();
            ICollection<Team> teams = teamEntities.Select(tm => teamConverter.ToTeam(tm.Team)).ToList();
            DateTime date = aMatch.Date;
            Sport sport = sportConverter.ToSport(aMatch.SportEntity);
            Match created = new Match(aMatch.Id,teams,date, sport, comments);
            return created;
        }

        public ICollection<MatchTeam> ConvertParticipants(Match aMatch)
        {
            MatchEntity matchEntity = ToEntity(aMatch);
            ICollection<MatchTeam> conversions = new List<MatchTeam>();
            foreach (Team participant in aMatch.GetParticipants()) {
                TeamEntity team = teamConverter.ToEntity(participant);
                MatchTeam participantConversion = new MatchTeam()
                {
                    Match = matchEntity,
                    MatchId = matchEntity.Id,
                    Team = team,
                    TeamNumber = team.TeamNumber
                };
                conversions.Add(participantConversion);
            }
            return conversions;
        }
    }
}

