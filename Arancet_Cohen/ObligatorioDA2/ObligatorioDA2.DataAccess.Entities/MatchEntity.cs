using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class MatchEntity:BaseEntity
    {
        public TeamEntity HomeTeam { get; set; }
        public TeamEntity AwayTeam { get; set; }
        public DateTime Date { get; set; }
        public ICollection<CommentEntity> Commentaries { get; set; }

        public int SportEntityId { get; set; }
        public SportEntity SportEntity { get; set; }
    }
}
