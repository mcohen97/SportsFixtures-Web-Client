using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.DataAccess.Entities
{
    class AdminEntity:UserEntity
    {
        public AdminEntity() {
            IsAdmin = true;
        }

        
    }
}
