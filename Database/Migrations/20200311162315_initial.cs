using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaddlePlan",
                columns: table => new
                {
                    PlanId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubscriptionType = table.Column<byte>(nullable: false),
                    Months = table.Column<int>(nullable: false),
                    MonthlyPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaddlePlan", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DailyMatchesLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionCancelled",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CancellationEffectiveDate = table.Column<DateTime>(nullable: false),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    Passthrough = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<int>(nullable: false),
                    SubscriptionPlanId = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionCancelled", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionCreated",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CancelUrl = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    NextBillDate = table.Column<DateTime>(nullable: false),
                    Passthrough = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<int>(nullable: false),
                    SubscriptionPlanId = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    UpdateUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionCreated", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPaymentFailed",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<string>(nullable: true),
                    CancelUrl = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    NextRetryDate = table.Column<DateTime>(nullable: false),
                    Passthrough = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<int>(nullable: false),
                    SubscriptionPlanId = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<string>(nullable: true),
                    UpdateUrl = table.Column<string>(nullable: true),
                    SubscriptionPaymentId = table.Column<string>(nullable: true),
                    Installments = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    AttemptNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPaymentFailed", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPaymentRefunded",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<string>(nullable: true),
                    BalanceCurrency = table.Column<string>(nullable: true),
                    BalanceEarningsDecrease = table.Column<string>(nullable: true),
                    BalanceFeeRefund = table.Column<string>(nullable: true),
                    BalanceGrossRefund = table.Column<string>(nullable: true),
                    BalanceTaxRefund = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    EarningsDecrease = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    FeeRefund = table.Column<string>(nullable: true),
                    GrossRefund = table.Column<string>(nullable: true),
                    InitialPayment = table.Column<string>(nullable: true),
                    Instalments = table.Column<string>(nullable: true),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    Passthrough = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    RefundReason = table.Column<string>(nullable: true),
                    RefundType = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionPaymentId = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    TaxRefund = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPaymentRefunded", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPaymentSucceeded",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BalanceCurrency = table.Column<string>(nullable: true),
                    BalanceEarnings = table.Column<string>(nullable: true),
                    BalanceFee = table.Column<string>(nullable: true),
                    BalanceGross = table.Column<string>(nullable: true),
                    BalanceTax = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Coupon = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    Earnings = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    Fee = table.Column<string>(nullable: true),
                    InitialPayment = table.Column<string>(nullable: true),
                    Instalments = table.Column<string>(nullable: true),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    NextBillDate = table.Column<DateTime>(nullable: false),
                    NextPaymentAmount = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true),
                    Passthrough = table.Column<string>(nullable: true),
                    PaymentTax = table.Column<string>(nullable: true),
                    PlanName = table.Column<string>(nullable: true),
                    Quantity = table.Column<string>(nullable: true),
                    ReceiptUrl = table.Column<string>(nullable: true),
                    SaleGross = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    SubscriptionPaymentId = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPaymentSucceeded", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionUpdated",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CancelUrl = table.Column<string>(nullable: true),
                    CheckoutId = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    MarketingConsent = table.Column<bool>(nullable: false),
                    Passthrough = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    UpdateUrl = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<int>(nullable: false),
                    PausedAt = table.Column<DateTime>(nullable: false),
                    PausedFrom = table.Column<DateTime>(nullable: false),
                    NewPrice = table.Column<string>(nullable: true),
                    OldPrice = table.Column<string>(nullable: true),
                    NewQuantity = table.Column<string>(nullable: true),
                    OldQuantity = table.Column<string>(nullable: true),
                    NewUnitPrice = table.Column<string>(nullable: true),
                    OldUnitPrice = table.Column<string>(nullable: true),
                    NextBillDate = table.Column<DateTime>(nullable: false),
                    OldNextBillDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    OldStatus = table.Column<string>(nullable: true),
                    SubscriptionPlanId = table.Column<int>(nullable: false),
                    OldSubscriptionPlanId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionUpdated", x => x.AlertId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    SteamId = table.Column<long>(nullable: false),
                    Registration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

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
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
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

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
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

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaddlePlanRole");

            migrationBuilder.DropTable(
                name: "PaddleSubscription");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SubscriptionCancelled");

            migrationBuilder.DropTable(
                name: "SubscriptionCreated");

            migrationBuilder.DropTable(
                name: "SubscriptionPaymentFailed");

            migrationBuilder.DropTable(
                name: "SubscriptionPaymentRefunded");

            migrationBuilder.DropTable(
                name: "SubscriptionPaymentSucceeded");

            migrationBuilder.DropTable(
                name: "SubscriptionUpdated");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "PaddlePlan");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
