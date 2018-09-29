using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using RepositoryInterface;

namespace DataRepositoryInterfaces
{
    public interface IMatchRepository
    {
        Match Get(int id);

        Match Add(string sportName, Match aMatch);

        void Delete(int id);

        Commentary CommentOnMatch(int idMatch, Commentary aComment);
    }
}
