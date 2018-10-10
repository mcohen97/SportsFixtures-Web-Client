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
                name: "Sports",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
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
                    table.PrimaryKey("PK_Teams", x => x.Identity);
                    table.UniqueConstraint("AK_Teams_SportEntityName_Name", x => new { x.SportEntityName, x.Name });
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
                    HomeTeamIdentity = table.Column<int>(nullable: true),
                    AwayTeamIdentity = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    SportEntityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_AwayTeamIdentity",
                        column: x => x.AwayTeamIdentity,
                        principalTable: "Teams",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamIdentity",
                        column: x => x.HomeTeamIdentity,
                        principalTable: "Teams",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Sports_SportEntityName",
                        column: x => x.SportEntityName,
                        principalTable: "Sports",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    UserEntityUserName = table.Column<string>(nullable: false),
                    TeamIdentity = table.Column<int>(nullable: true),
                    TeamEntityName = table.Column<string>(nullable: false),
                    TeamEntitySportEntityName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeams", x => new { x.TeamEntityName, x.TeamEntitySportEntityName, x.UserEntityUserName });
                    table.ForeignKey(
                        name: "FK_UserTeams_Teams_TeamIdentity",
                        column: x => x.TeamIdentity,
                        principalTable: "Teams",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTeams_Users_UserEntityUserName",
                        column: x => x.UserEntityUserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MakerUserName = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    MatchEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_MakerUserName",
                        column: x => x.MakerUserName,
                        principalTable: "Users",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Matches_MatchEntityId",
                        column: x => x.MatchEntityId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MakerUserName",
                table: "Comments",
                column: "MakerUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MatchEntityId",
                table: "Comments",
                column: "MatchEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamIdentity",
                table: "Matches",
                column: "AwayTeamIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamIdentity",
                table: "Matches",
                column: "HomeTeamIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SportEntityName",
                table: "Matches",
                column: "SportEntityName");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamIdentity",
                table: "UserTeams",
                column: "TeamIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UserEntityUserName",
                table: "UserTeams",
                column: "UserEntityUserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Sports");
        }
    }
}
