﻿using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;
using RepositoryInterface;

namespace DataRepositoryInterfaces
{
    public interface IMatchRepository : IRepository<Match, int>
    {

        Commentary CommentOnMatch(int idMatch, Commentary aComment);
        ICollection<Commentary> GetComments();
    }
}
