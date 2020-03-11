using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class subscriptioncenteredinsteadofpaddleuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaddlePlan_Roles_RoleId",
                table: "PaddlePlan");

            migrationBuilder.DropTable(
                name: "PaddleUser");

            migrationBuilder.DropIndex(
                name: "IX_PaddlePlan_RoleId",
                table: "PaddlePlan");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "PaddlePlan");

            migrationBuilder.CreateTable(
                name: "PaddlePlanRole",
                columns: table => new
                {
                    PlanId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaddlePlanRole", x => new { x.PlanId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_PaddlePlanRole_PaddlePlan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "PaddlePlan",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaddlePlanRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaddleSubscription",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<int>(nullable: false),
                    SubscriptionPlanId = table.Column<int>(nullable: false),
                    CancelUrl = table.Column<string>(nullable: true),
                    UpdateUrl = table.Column<string>(nullable: true),
                    ExpirationTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaddleSubscription", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_PaddleSubscription_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaddleSubscription_PaddlePlan_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "PaddlePlan",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaddlePlanRole_RoleId",
                table: "PaddlePlanRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PaddleSubscription_ApplicationUserId",
                table: "PaddleSubscription",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaddlePlanRole");

            migrationBuilder.DropTable(
                name: "PaddleSubscription");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "PaddlePlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaddleUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    CancelUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    MarketingConsent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Passthrough = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Status = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionPlanId = table.Column<int>(type: "int", nullable: false),
                    UpdateUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaddleUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaddleUser_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaddlePlan_RoleId",
                table: "PaddlePlan",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PaddleUser_ApplicationUserId",
                table: "PaddleUser",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaddlePlan_Roles_RoleId",
                table: "PaddlePlan",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
