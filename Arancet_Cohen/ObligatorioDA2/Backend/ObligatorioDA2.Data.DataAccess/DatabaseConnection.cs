using Microsoft.EntityFrameworkCore;
using ObligatorioDA2.Data.Entities;

namespace ObligatorioDA2.Data.DataAccess
{
    public class DatabaseConnection : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<TeamEntity> Teams { get; set; }
        public virtual DbSet<EncounterEntity> Matches { get; set; }
        public virtual DbSet<CommentEntity> Comments { get; set; }
        public virtual DbSet<SportEntity> Sports { get; set; }
        public virtual DbSet<LogInfoEntity> Logs { get; set; }
        public virtual DbSet<UserTeam> UserTeams { get; set; }
        public virtual DbSet<EncounterTeam> MatchTeams { get; set; }


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
            modelBuilder.Entity<EncounterTeam>().HasKey(mt => new { mt.MatchId, mt.TeamNumber });
            modelBuilder.Entity<LogInfoEntity>().HasKey(li => li.Id);


            modelBuilder.Entity<TeamEntity>().Property(u => u.TeamNumber).ValueGeneratedOnAdd();
            modelBuilder.Entity<EncounterEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CommentEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<LogInfoEntity>().Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}
