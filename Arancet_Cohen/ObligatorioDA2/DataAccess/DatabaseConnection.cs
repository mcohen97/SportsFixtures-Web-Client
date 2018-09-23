using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ObligatorioDA2.DataAccess.Entities;

namespace DataAccess
{
    public class DatabaseConnection : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<TeamEntity> Teams { get; set; }

        public DatabaseConnection(DbContextOptions<DatabaseConnection> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<UserEntity>().HasKey(u => u.UserName);
<<<<<<< HEAD
            modelBuilder.Entity<TeamEntity>().HasKey(t => t.Id);
            modelBuilder.Entity<TeamEntity>().HasAlternateKey(t => t.Name);
=======
            modelBuilder.Entity<UserEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
>>>>>>> b4000f454dcd8e99986515f6c43e4aeff9e05987

        }
    }
}
