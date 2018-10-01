using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using RepositoryInterface;

namespace DataRepositoryInterfaces
{
    public interface IMatchRepository : IRepository<Match, int>
    {

        bool Exists(int id);

        Commentary CommentOnMatch(int idMatch, Commentary aComment);

    }
}
