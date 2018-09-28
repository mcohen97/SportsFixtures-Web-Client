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
        public virtual DbSet<MatchEntity> Matches { get; set; }
        public virtual DbSet<CommentEntity> Comments { get;set; }
        public virtual DbSet<SportEntity> Sports { get; set; }

        public DatabaseConnection(DbContextOptions<DatabaseConnection> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SportEntity>().HasKey(s => s.Id);
            modelBuilder.Entity<TeamEntity>().HasAlternateKey(t => t.Name);
            modelBuilder.Entity<UserEntity>().HasIndex(u => u.UserName).IsUnique();
            //modelBuilder.Entity<UserEntity>().HasAlternateKey(u => u.UserName);
            //modelBuilder.Entity<UserEntity>().HasKey(u => u.UserName);
            //modelBuilder.Entity<CommentEntity>().HasOne<UserEntity>(ce => ce.Maker);

            modelBuilder.Entity<SportEntity>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TeamEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<MatchEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CommentEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
        }   
    }
}
