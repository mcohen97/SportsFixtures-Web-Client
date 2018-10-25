using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class MatchTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_AwayTeamTeamNumber",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_HomeTeamTeamNumber",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_AwayTeamTeamNumber",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_HomeTeamTeamNumber",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AwayTeamTeamNumber",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeTeamTeamNumber",
                table: "Matches");

            migrationBuilder.AddColumn<bool>(
                name: "IsTwoTeams",
                table: "Sports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MatchTeams",
                columns: table => new
                {
                    TeamNumber = table.Column<int>(nullable: false),
                    MatchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTeams", x => new { x.MatchId, x.TeamNumber });
                    table.ForeignKey(
                        name: "FK_MatchTeams_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchTeams_Teams_TeamNumber",
                        column: x => x.TeamNumber,
                        principalTable: "Teams",
                        principalColumn: "TeamNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeams_TeamNumber",
                table: "MatchTeams",
                column: "TeamNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchTeams");

            migrationBuilder.DropColumn(
                name: "IsTwoTeams",
                table: "Sports");

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamTeamNumber",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamTeamNumber",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamTeamNumber",
                table: "Matches",
                column: "AwayTeamTeamNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamTeamNumber",
                table: "Matches",
                column: "HomeTeamTeamNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_AwayTeamTeamNumber",
                table: "Matches",
                column: "AwayTeamTeamNumber",
                principalTable: "Teams",
                principalColumn: "TeamNumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_HomeTeamTeamNumber",
                table: "Matches",
                column: "HomeTeamTeamNumber",
                principalTable: "Teams",
                principalColumn: "TeamNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
