using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.DataAccess.Entities
{
    public class AdminEntity:UserEntity
    {
        public AdminEntity():base()
        {
            IsAdmin = true;
        }    
    }
}
