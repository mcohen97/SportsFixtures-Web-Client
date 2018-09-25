using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class MatchEntity:BaseEntity
    {
        public TeamEntity HomeTeam;
        public TeamEntity AwayTeam;
        public DateTime Date;
        public ICollection<CommentEntity> Commentaries;
    }
}
