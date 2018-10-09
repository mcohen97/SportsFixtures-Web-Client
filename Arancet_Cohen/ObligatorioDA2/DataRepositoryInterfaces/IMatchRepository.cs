using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Data.Repositories.Interfaces
{
    public interface IMatchRepository : IRepository<Match, int>
    {

        Commentary CommentOnMatch(int idMatch, Commentary aComment);
        Commentary GetComment(int id);
        ICollection<Commentary> GetComments();
    }
}
