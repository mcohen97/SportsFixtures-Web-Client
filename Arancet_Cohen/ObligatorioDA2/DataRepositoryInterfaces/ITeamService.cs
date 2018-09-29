using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic;

namespace DataRepositoryInterfaces
{
    public interface ITeamService
    {
        void AddTeam(Sport played, Team testTeam);
    }
}
