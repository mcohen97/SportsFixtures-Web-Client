using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic
{
    public class Result
    {
        private ICollection<Tuple<Team, int>> positions;

        public Result() {
            positions = new List<Tuple<Team, int>>();
        }

        public void Add(Team testTeam, int position)
        {
            positions.Add(new Tuple<Team, int>(testTeam, position));
        }

        public ICollection<Tuple<Team, int>> GetPositions() {
            return positions;
        }
    }
}
