using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class AddPaddlePlanForeignKeytoApplicationRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PaddlePlan_RoleId",
                table: "PaddlePlan",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaddlePlan_Roles_RoleId",
                table: "PaddlePlan",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaddlePlan_Roles_RoleId",
                table: "PaddlePlan");

            migrationBuilder.DropIndex(
                name: "IX_PaddlePlan_RoleId",
                table: "PaddlePlan");
        }
    }
}
