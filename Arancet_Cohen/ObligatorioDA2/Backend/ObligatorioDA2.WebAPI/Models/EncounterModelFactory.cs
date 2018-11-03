using ObligatorioDA2.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObligatorioDA2.WebAPI.Models
{
    public class EncounterModelFactory
    {
        public EncounterModelOut CreateModelOut(Encounter encounter) {
            EncounterModelOut conversion;
            if (encounter.Sport.IsTwoTeams)
            {
                conversion = CreateMatchModelOut(encounter);
            }
            else {
                conversion = CreateCompetitionModelOut(encounter);
            }
            return conversion;
        }

        private MatchModelOut CreateMatchModelOut(Encounter encounter)
        {
            MatchModelOut converted = new MatchModelOut()
            {
                Id = encounter.Id,
                SportName = encounter.Sport.Name,
                TeamsIds = encounter.GetParticipants().Select(p => p.Id).ToList(),
                Date = encounter.Date,
                CommentsIds = encounter.GetAllCommentaries().Select(c => c.Id).ToList(),
                HasResult = encounter.HasResult()
            };
            if (encounter.HasResult()) {
                List<Tuple<Team, int>> standings = encounter.Result.GetPositions().ToList();
                converted.HasWinner = standings[0].Item2 != standings[1].Item2;
                if (converted.HasWinner)
                {
                    converted.WinnerId = standings.First(t => t.Item2 == 1).Item1.Id;
                }
            }
            return converted;
        }

        private CompetitionModelOut CreateCompetitionModelOut(Encounter encounter)
        {
            CompetitionModelOut converted = new CompetitionModelOut()
            {
                Id = encounter.Id,
                TeamsIds = encounter.GetParticipants().Select(p => p.Id).ToList(),
                Date = encounter.Date,
                SportName = encounter.Sport.Name,
                CommentsIds = encounter.GetAllCommentaries().Select(c => c.Id).ToList(),
                HasResult = encounter.HasResult()
            };
            if (encounter.HasResult()) {
                converted.Team_Position = encounter.Result.GetPositions()
                                .Select(p => new StandingModelOut() { TeamName = p.Item1.Name, TeamId = p.Item1.Id,
                                                                       Points= p.Item2 })
                                .ToList();
            }
            return converted;
        }


    }
}
