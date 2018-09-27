using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_UserName",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "SportEntityId",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SportEntityId",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SportEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SportEntityId",
                table: "Teams",
                column: "SportEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SportEntityId",
                table: "Matches",
                column: "SportEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_SportEntity_SportEntityId",
                table: "Matches",
                column: "SportEntityId",
                principalTable: "SportEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_SportEntity_SportEntityId",
                table: "Teams",
                column: "SportEntityId",
                principalTable: "SportEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_SportEntity_SportEntityId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_SportEntity_SportEntityId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "SportEntity");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SportEntityId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Matches_SportEntityId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SportEntityId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "SportEntityId",
                table: "Matches");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_UserName",
                table: "Users",
                column: "UserName");
        }
    }
}
