using System;
using System.Collections.Generic;
using System.Text;
using ObligatorioDA2.BusinessLogic;

namespace ObligatorioDA2.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User, string>
    {
        User Get(User asked);
    }
}
