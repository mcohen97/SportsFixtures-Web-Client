using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.Data.Entities;

namespace ObligatorioDA2.Data.DataAccess
{
    public class DatabaseConnection : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<TeamEntity> Teams { get; set; }
        public virtual DbSet<MatchEntity> Matches { get; set; }
        public virtual DbSet<CommentEntity> Comments { get; set; }
        public virtual DbSet<SportEntity> Sports { get; set; }

        public virtual DbSet<UserTeam> UserTeams { get; set; }

        public DatabaseConnection(DbContextOptions<DatabaseConnection> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SportEntity>().HasKey(t => t.Name);
            modelBuilder.Entity<UserEntity>().HasKey(u => u.UserName);
            modelBuilder.Entity<TeamEntity>().HasKey(t => t.TeamNumber);
            modelBuilder.Entity<TeamEntity>().HasAlternateKey(t => new { t.SportEntityName, t.Name });
            modelBuilder.Entity<UserTeam>().HasKey(ut => new { ut.TeamEntityName, ut.TeamEntitySportEntityName, ut.UserEntityUserName });



            modelBuilder.Entity<TeamEntity>().Property(u => u.TeamNumber).ValueGeneratedOnAdd();
            modelBuilder.Entity<MatchEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CommentEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}
