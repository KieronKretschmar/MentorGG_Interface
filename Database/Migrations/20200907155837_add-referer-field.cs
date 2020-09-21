using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class addrefererfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RefererSteamId",
                table: "Users",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefererSteamId",
                table: "Users");
        }
    }
}
