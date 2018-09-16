using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.DataAccess.Entities
{
    class FollowerEntity:UserEntity
    {
        public FollowerEntity() {
            IsAdmin = false;
        }
    }
}
