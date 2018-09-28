using BusinessLogic;
using RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositoryInterfaces
{
    public interface ISportRepository:IRepository<Sport>
    {
        Sport GetSportByName(string name);
     }
}
