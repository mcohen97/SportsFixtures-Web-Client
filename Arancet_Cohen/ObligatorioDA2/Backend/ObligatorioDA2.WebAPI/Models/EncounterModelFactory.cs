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

        private MatchModelOut CreateCompetitionModelOut(Encounter encounter)
        {
            MatchModelOut converted = new MatchModelOut()
            {
                Id = encounter.Id,
                TeamsIds = encounter.GetParticipants().Select(p => p.Id).ToList(),
                Date = encounter.Date,
                SportName = encounter.Sport.Name,
                CommentsIds = encounter.GetAllCommentaries().Select(c => c.Id).ToList(),
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

        private CompetitionModelOut CreateMatchModelOut(Encounter encounter)
        {
            CompetitionModelOut converted = new CompetitionModelOut()
            {
                Id = encounter.Id,
                TeamsIds = encounter.GetParticipants().Select(p => p.Id).ToList(),
                Date = encounter.Date,
                SportName = encounter.Sport.Name,
                CommentsIds = encounter.GetAllCommentaries().Select(c => c.Id).ToList(),
                Team_Position = encounter.Result.GetPositions()
                                .Select(p => new Tuple<int,int>(p.Item1.Id,p.Item2))
                                .ToList()
            };
            return converted;
        }


    }
}
