using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BusinessLogic;

namespace DataAccess
{
    public class DatabaseConnection : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public DatabaseConnection()
        {
        }
        public DatabaseConnection(DbContextOptions<DatabaseConnection> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.UserName);

        }
    }
}
