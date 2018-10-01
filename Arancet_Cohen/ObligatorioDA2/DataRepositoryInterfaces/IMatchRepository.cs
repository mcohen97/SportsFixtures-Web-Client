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

        int Add(Match aMatch);

        void Modify(Match aMatch);

        void Delete(int id);

        Commentary CommentOnMatch(int idMatch, Commentary aComment);

        ICollection<Match> GetAll();
    }
}
