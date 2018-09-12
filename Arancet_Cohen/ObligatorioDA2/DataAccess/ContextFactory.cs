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
            dbContextType = typeof(TDbContext);
            this.dbContext = dbContext;
        }

        public DbContextT Get<DbContextT>() where DbContextT : DbContext, new()
        {
            DbContextT creation;
            if (dbContext == null || dbContextType != typeof(DbContextT))
            {
                creation = new DbContextT();
            }
            creation = (DbContextT)dbContext;
            return creation;
        }
    }
}
