using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using RepositoryInterface;

namespace DataRepositoryInterfaces
{
    public interface IMatchRepository
    {
        void Clear();

        bool IsEmpty();
        bool Exists(int id);

        Match Get(int id);

        Match Add(string sportName, Match aMatch);

        void Modify(string sportName, Match aMatch);

        void Delete(int id);

        Commentary CommentOnMatch(int idMatch, Commentary aComment);

        ICollection<Match> GetAll();
    }
}
