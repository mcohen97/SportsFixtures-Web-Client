using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObligatorioDA2.BusinessLogic;
using ObligatorioDA2.Data.Entities;

namespace ObligatorioDA2.Data.DomainMappers
{
    public class EncounterMapper
    {
        private TeamMapper teamConverter;
        private CommentMapper commentConverter;
        private SportMapper sportConverter;
        private EncounterFactory factory;

        public EncounterMapper()
        {
            teamConverter = new TeamMapper();
            commentConverter = new CommentMapper();
            sportConverter = new SportMapper();
            factory = new EncounterFactory();
        }
        public MatchEntity ToEntity(Encounter aMatch)
        {
            SportEntity sportEntity = sportConverter.ToEntity(aMatch.Sport);
            MatchEntity conversion = new MatchEntity()
            {
                Id = aMatch.Id,
                Date = aMatch.Date,
                Commentaries = TransformCommentaries(aMatch.GetAllCommentaries()),
                SportEntity = sportEntity,
                HasResult = aMatch.HasResult()
            };
            return conversion;
        }


        private ICollection<CommentEntity> TransformCommentaries(ICollection<Commentary> commentaries)
        {
            return commentaries.Select(c => commentConverter.ToEntity(c)).ToList();
        }

        public Encounter ToEncounter(MatchEntity aMatch, ICollection<MatchTeam> playingTeams)
        {
            ICollection<Commentary> comments = aMatch.Commentaries.Select(ce => commentConverter.ToComment(ce)).ToList();
            ICollection<Team> teams = playingTeams.Select(tm => teamConverter.ToTeam(tm.Team)).ToList();
            DateTime date = aMatch.Date;
            Sport sport = sportConverter.ToSport(aMatch.SportEntity);
            Encounter created = factory.CreateEncounter(aMatch.Id,teams,date, sport, comments);
            if (aMatch.HasResult) {
                Result matchResult = ToResults(playingTeams);
                created.Result=matchResult;
            }
            return created;
        }

        private Result ToResults(ICollection<MatchTeam> playingTeams)
        {
            Result matchResult = new Result();
            foreach (MatchTeam mt in playingTeams) {
                Team converted = teamConverter.ToTeam(mt.Team);
                matchResult.Add(converted, mt.Position);
            }
            return matchResult;
        }

        public ICollection<MatchTeam> ConvertParticipants(Encounter aMatch)
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
            if (aMatch.HasResult()) {
                ResultsToEntity(conversions, aMatch.Result);
            }
            return conversions;
        }

        private void ResultsToEntity(ICollection<MatchTeam> conversions, Result result)
        {
            foreach (Tuple<Team, int> standing in result.GetPositions()) {
                conversions.First(mt => mt.TeamNumber == standing.Item1.Id).Position = standing.Item2;
            }
        }
    }
}

