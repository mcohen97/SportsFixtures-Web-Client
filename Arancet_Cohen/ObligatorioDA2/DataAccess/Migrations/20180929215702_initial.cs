using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    MatchEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Identity = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: true),
                    SportEntityName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => new { x.SportEntityName, x.Name });
                    table.ForeignKey(
                        name: "FK_Teams_Sports_SportEntityName",
                        column: x => x.SportEntityName,
                        principalTable: "Sports",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HomeTeamSportEntityName = table.Column<string>(nullable: true),
                    HomeTeamName = table.Column<string>(nullable: true),
                    AwayTeamSportEntityName = table.Column<string>(nullable: true),
                    AwayTeamName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_AwayTeamSportEntityName_AwayTeamName",
                        columns: x => new { x.AwayTeamSportEntityName, x.AwayTeamName },
                        principalTable: "Teams",
                        principalColumns: new[] { "SportEntityName", "Name" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamSportEntityName_HomeTeamName",
                        columns: x => new { x.HomeTeamSportEntityName, x.HomeTeamName },
                        principalTable: "Teams",
                        principalColumns: new[] { "SportEntityName", "Name" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MatchEntityId",
                table: "Comments",
                column: "MatchEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamSportEntityName_AwayTeamName",
                table: "Matches",
                columns: new[] { "AwayTeamSportEntityName", "AwayTeamName" });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamSportEntityName_HomeTeamName",
                table: "Matches",
                columns: new[] { "HomeTeamSportEntityName", "HomeTeamName" });

            migrationBuilder.CreateIndex(
                name: "IX_Sports_Id",
                table: "Sports",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Comments_Id",
                table: "Users",
                column: "Id",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Matches_MatchEntityId",
                table: "Comments",
                column: "MatchEntityId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sports_Matches_Id",
                table: "Sports",
                column: "Id",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sports_Matches_Id",
                table: "Sports");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Sports");
        }
    }
}
