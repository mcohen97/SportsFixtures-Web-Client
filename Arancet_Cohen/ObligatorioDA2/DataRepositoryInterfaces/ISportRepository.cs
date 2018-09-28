using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositoryInterfaces
{
    public interface ISportRepository
    {
        Sport GetSportByName(string name);
     }
}
