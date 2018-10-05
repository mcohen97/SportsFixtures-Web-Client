using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Exceptions
{
    public class TeamNotFollowedException:Exception
    {
        public TeamNotFollowedException():base("You don't follow this team") {}
    }
}
