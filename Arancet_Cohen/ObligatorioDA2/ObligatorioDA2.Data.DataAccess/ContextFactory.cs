using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class ContextFactory
    {
        private bool isSpecific;
        private DbContextOptions<DatabaseConnection> options;

        public ContextFactory() {
            isSpecific = false;
        }

        public ContextFactory(DbContextOptions<DatabaseConnection> someOptions){
            options = someOptions;
            isSpecific=true;
        }
        public DatabaseConnection Get()
        {
            DatabaseConnection toBuild;
            toBuild= new DatabaseConnection(options);
           
             return toBuild;
        }
    }
}
