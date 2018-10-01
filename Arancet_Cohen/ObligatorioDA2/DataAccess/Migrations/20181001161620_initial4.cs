using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_AwayTeamSportEntityName_AwayTeamName",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Teams_HomeTeamSportEntityName_HomeTeamName",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTeams_Teams_TeamEntitySportEntityName_TeamEntityName",
                table: "UserTeams");

            migrationBuilder.DropIndex(
                name: "IX_UserTeams_TeamEntitySportEntityName_TeamEntityName",
                table: "UserTeams");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Teams_Identity",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Matches_AwayTeamSportEntityName_AwayTeamName",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_HomeTeamSportEntityName_HomeTeamName",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AwayTeamName",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "AwayTeamSportEntityName",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeTeamName",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeTeamSportEntityName",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "TeamIdentity",
                table: "UserTeams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamIdentity",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamIdentity",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "Identity");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Teams_SportEntityName_Name",
                table: "Teams",
                columns: new[] { "SportEntityName", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamIdentity",
                table: "UserTeams",
                column: "TeamIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamIdentity",
                table: "Matches",
                column: "AwayTeamIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamIdentity",
                table: "Matches",
                column: "HomeTeamIdentity");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_UserTeams_TeamIdentity",
                table: "UserTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Teams_SportEntityName_Name",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Matches_AwayTeamIdentity",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_HomeTeamIdentity",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TeamIdentity",
                table: "UserTeams");

            migrationBuilder.DropColumn(
                name: "AwayTeamIdentity",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeTeamIdentity",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "AwayTeamName",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AwayTeamSportEntityName",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamName",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamSportEntityName",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Teams_Identity",
                table: "Teams",
                column: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                columns: new[] { "SportEntityName", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamEntitySportEntityName_TeamEntityName",
                table: "UserTeams",
                columns: new[] { "TeamEntitySportEntityName", "TeamEntityName" });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamSportEntityName_AwayTeamName",
                table: "Matches",
                columns: new[] { "AwayTeamSportEntityName", "AwayTeamName" });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamSportEntityName_HomeTeamName",
                table: "Matches",
                columns: new[] { "HomeTeamSportEntityName", "HomeTeamName" });

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_AwayTeamSportEntityName_AwayTeamName",
                table: "Matches",
                columns: new[] { "AwayTeamSportEntityName", "AwayTeamName" },
                principalTable: "Teams",
                principalColumns: new[] { "SportEntityName", "Name" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Teams_HomeTeamSportEntityName_HomeTeamName",
                table: "Matches",
                columns: new[] { "HomeTeamSportEntityName", "HomeTeamName" },
                principalTable: "Teams",
                principalColumns: new[] { "SportEntityName", "Name" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTeams_Teams_TeamEntitySportEntityName_TeamEntityName",
                table: "UserTeams",
                columns: new[] { "TeamEntitySportEntityName", "TeamEntityName" },
                principalTable: "Teams",
                principalColumns: new[] { "SportEntityName", "Name" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
