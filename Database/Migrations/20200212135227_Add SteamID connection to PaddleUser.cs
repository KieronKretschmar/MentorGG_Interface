using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddSteamIDconnectiontoPaddleUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Passthrough",
                table: "PaddleUser",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SteamId",
                table: "PaddleUser",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_SteamId",
                table: "Users",
                column: "SteamId");

            migrationBuilder.CreateIndex(
                name: "IX_PaddleUser_SteamId",
                table: "PaddleUser",
                column: "SteamId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaddleUser_Users_SteamId",
                table: "PaddleUser",
                column: "SteamId",
                principalTable: "Users",
                principalColumn: "SteamId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaddleUser_Users_SteamId",
                table: "PaddleUser");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_SteamId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_PaddleUser_SteamId",
                table: "PaddleUser");

            migrationBuilder.DropColumn(
                name: "Passthrough",
                table: "PaddleUser");

            migrationBuilder.DropColumn(
                name: "SteamId",
                table: "PaddleUser");
        }
    }
}
