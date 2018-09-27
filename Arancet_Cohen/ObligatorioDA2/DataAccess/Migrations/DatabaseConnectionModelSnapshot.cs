﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(DatabaseConnection))]
    partial class DatabaseConnectionModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ObligatorioDA2.DataAccess.Entities.CommentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MakerId");

                    b.Property<int?>("MatchEntityId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("MakerId");

                    b.HasIndex("MatchEntityId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ObligatorioDA2.DataAccess.Entities.MatchEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AwayTeamId");

                    b.Property<DateTime>("Date");

                    b.Property<int?>("HomeTeamId");

                    b.Property<int>("SportEntityId");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("SportEntityId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("ObligatorioDA2.DataAccess.Entities.SportEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("SportEntity");
                });

            modelBuilder.Entity("ObligatorioDA2.DataAccess.Entities.TeamEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Photo");

                    b.Property<int>("SportEntityId");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("SportEntityId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ObligatorioDA2.DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<bool>("IsAdmin");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Surname");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ObligatorioDA2.DataAccess.Entities.CommentEntity", b =>
                {
                    b.HasOne("ObligatorioDA2.DataAccess.Entities.UserEntity", "Maker")
                        .WithMany()
                        .HasForeignKey("MakerId");

                    b.HasOne("ObligatorioDA2.DataAccess.Entities.MatchEntity")
                        .WithMany("Commentaries")
                        .HasForeignKey("MatchEntityId");
                });

            modelBuilder.Entity("ObligatorioDA2.DataAccess.Entities.MatchEntity", b =>
                {
                    b.HasOne("ObligatorioDA2.DataAccess.Entities.TeamEntity", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("ObligatorioDA2.DataAccess.Entities.TeamEntity", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId");

                    b.HasOne("ObligatorioDA2.DataAccess.Entities.SportEntity", "SportEntity")
                        .WithMany()
                        .HasForeignKey("SportEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ObligatorioDA2.DataAccess.Entities.TeamEntity", b =>
                {
                    b.HasOne("ObligatorioDA2.DataAccess.Entities.SportEntity")
                        .WithMany("Teams")
                        .HasForeignKey("SportEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
