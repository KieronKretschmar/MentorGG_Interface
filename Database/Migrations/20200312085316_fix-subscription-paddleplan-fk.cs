using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class fixsubscriptionpaddleplanfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaddleSubscription_PaddlePlan_SubscriptionId",
                table: "PaddleSubscription");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "PaddleSubscription",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_PaddleSubscription_SubscriptionPlanId",
                table: "PaddleSubscription",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaddleSubscription_PaddlePlan_SubscriptionPlanId",
                table: "PaddleSubscription",
                column: "SubscriptionPlanId",
                principalTable: "PaddlePlan",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaddleSubscription_PaddlePlan_SubscriptionPlanId",
                table: "PaddleSubscription");

            migrationBuilder.DropIndex(
                name: "IX_PaddleSubscription_SubscriptionPlanId",
                table: "PaddleSubscription");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "PaddleSubscription",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_PaddleSubscription_PaddlePlan_SubscriptionId",
                table: "PaddleSubscription",
                column: "SubscriptionId",
                principalTable: "PaddlePlan",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
