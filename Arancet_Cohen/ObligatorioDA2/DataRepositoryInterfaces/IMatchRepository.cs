using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using RepositoryInterface;

namespace DataRepositoryInterfaces
{
    public interface IMatchRepository : IRepository<Match, int>
    {

        Commentary CommentOnMatch(int idMatch, Commentary aComment);
        Commentary GetComment(int id);
        ICollection<Commentary> GetComments();
    }
}
