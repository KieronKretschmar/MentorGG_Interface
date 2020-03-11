using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class addpaddleplansubscriptiontype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MonthlyPrice",
                table: "PaddlePlan",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Months",
                table: "PaddlePlan",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "SubscriptionType",
                table: "PaddlePlan",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyPrice",
                table: "PaddlePlan");

            migrationBuilder.DropColumn(
                name: "Months",
                table: "PaddlePlan");

            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "PaddlePlan");
        }
    }
}
