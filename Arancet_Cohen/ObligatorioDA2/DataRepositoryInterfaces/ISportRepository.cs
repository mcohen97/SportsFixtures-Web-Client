using BusinessLogic;
using RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepositoryInterfaces
{
    public interface ISportRepository:IRepository<Sport,string>
    {
        Sport Get(string name);
     }
}
