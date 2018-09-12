using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class ContextFactory
    {
        private Type dbContextType;
        private DbContext dbContext;

        public void Register<TDbContext>(TDbContext dbContext) where TDbContext : DbContext, new()
        {
            
        }

        public DbContextT Get<DbContextT>() where DbContextT : DbContext, new()
        {
            return new DbContextT();
        }
    }
}
