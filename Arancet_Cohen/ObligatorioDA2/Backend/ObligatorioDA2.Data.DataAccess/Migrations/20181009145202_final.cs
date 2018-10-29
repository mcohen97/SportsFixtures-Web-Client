using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_AwayTeamIdentity",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_HomeTeamIdentity",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeams_Teams_TeamIdentity",
                table: "UserTeams");

            migrationBuilder.RenameColumn(
                name: "TeamIdentity",
                table: "UserTeams",
                newName: "TeamNumber");

            migrationBuilder.RenameIndex(
                name: "IX_UserTeams_TeamIdentity",
                table: "UserTeams",
                newName: "IX_UserTeams_TeamNumber");

            migrationBuilder.RenameColumn(
                name: "Identity",
                table: "Teams",
                newName: "TeamNumber");

            migrationBuilder.RenameColumn(
                name: "HomeTeamIdentity",
                table: "Matches",
                newName: "HomeTeamTeamNumber");

            migrationBuilder.RenameColumn(
                name: "AwayTeamIdentity",
                table: "Matches",
                newName: "AwayTeamTeamNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_HomeTeamIdentity",
                table: "Matches",
                newName: "IX_Matches_HomeTeamTeamNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_AwayTeamIdentity",
                table: "Matches",
                newName: "IX_Matches_AwayTeamTeamNumber");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeams_Teams_TeamNumber",
                table: "UserTeams",
                column: "TeamNumber",
                principalTable: "Teams",
                principalColumn: "TeamNumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_AwayTeamTeamNumber",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_HomeTeamTeamNumber",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeams_Teams_TeamNumber",
                table: "UserTeams");

            migrationBuilder.RenameColumn(
                name: "TeamNumber",
                table: "UserTeams",
                newName: "TeamIdentity");

            migrationBuilder.RenameIndex(
                name: "IX_UserTeams_TeamNumber",
                table: "UserTeams",
                newName: "IX_UserTeams_TeamIdentity");

            migrationBuilder.RenameColumn(
                name: "TeamNumber",
                table: "Teams",
                newName: "Identity");

            migrationBuilder.RenameColumn(
                name: "HomeTeamTeamNumber",
                table: "Matches",
                newName: "HomeTeamIdentity");

            migrationBuilder.RenameColumn(
                name: "AwayTeamTeamNumber",
                table: "Matches",
                newName: "AwayTeamIdentity");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_HomeTeamTeamNumber",
                table: "Matches",
                newName: "IX_Matches_HomeTeamIdentity");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_AwayTeamTeamNumber",
                table: "Matches",
                newName: "IX_Matches_AwayTeamIdentity");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_AwayTeamIdentity",
                table: "Matches",
                column: "AwayTeamIdentity",
                principalTable: "Teams",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_HomeTeamIdentity",
                table: "Matches",
                column: "HomeTeamIdentity",
                principalTable: "Teams",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeams_Teams_TeamIdentity",
                table: "UserTeams",
                column: "TeamIdentity",
                principalTable: "Teams",
                principalColumn: "Identity",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
