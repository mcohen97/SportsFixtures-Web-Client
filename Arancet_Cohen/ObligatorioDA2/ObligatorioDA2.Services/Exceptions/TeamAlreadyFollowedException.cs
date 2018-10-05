using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services.Exceptions
{
    public class TeamAlreadyFollowedException:Exception
    {
        public TeamAlreadyFollowedException():base("User already follows team") {

        }
    }
}
